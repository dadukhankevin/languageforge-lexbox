<script lang="ts" context="module">
  export const enum DialogResponse {
    Cancel = 'cancel',
    Submit = 'submit',
  }
</script>

<script lang="ts">
  import t from '$lib/i18n';
  import { createEventDispatcher } from 'svelte';
  import { writable } from 'svelte/store';

  const dispatch = createEventDispatcher<{
    close: DialogResponse;
    open: void;
    submit: void;
  }>();

  let dialogResponse = writable<DialogResponse | null>(null);
  let open = writable(false);
  $: closing = $dialogResponse !== null && $open;
  export let bottom = false;
  export let showCloseButton = true;
  export async function openModal(autoCloseOnCancel = true, autoCloseOnSubmit = false): Promise<DialogResponse> {
    $dialogResponse = null;
    $open = true;
    dispatch('open');
    const response = await new Promise<DialogResponse>((resolve) => {
      const unsub = dialogResponse.subscribe((reason) => {
        if (reason) {
          unsub();
          resolve(reason);
        }
      });
    });
    if (autoCloseOnCancel && response === DialogResponse.Cancel) {
      close();
    }
    if (autoCloseOnSubmit && response === DialogResponse.Submit) {
      close();
    }
    return response;
  }
  export function cancelModal(): void {
    $dialogResponse = DialogResponse.Cancel;
  }
  export function submitModal(): Promise<void> {
    $dialogResponse = DialogResponse.Submit;
    //a promise that will resolve when the modal is closed, or openModal is called again
    return new Promise<void>((resolve) => {
      const unsubOpen = open.subscribe((open) => {
        if (!open) {
          unsubOpen();
          resolve();
        }
      });
      const unsubResponse = dialogResponse.subscribe((reason) => {
        if (reason === null) {
          unsubResponse();
          resolve();
        }
      });
    });
  }

  export function close(): void {
    $open = false;
  }

  $: if ($dialogResponse === DialogResponse.Submit) {
    dispatch('submit');
  }
  $: if (!$open && $dialogResponse !== null) {
    dispatch('close', $dialogResponse);
  }
  let dialog: HTMLDialogElement | undefined;
  //dialog will still work if the browser doesn't support it, but this enables focus trapping and other features
  $: if (dialog) {
    if ($open) {
      //showModal might be undefined if the browser doesn't support dialog
      dialog.showModal?.call(dialog);
    } else {
      dialog.close?.call(dialog);
    }
  }
</script>

{#if $open}
  <!-- using DaisyUI modal https://daisyui.com/components/modal/ -->
  <div class="modal" class:modal-bottom={bottom} class:modal-open={$open}>
    <dialog bind:this={dialog} class="modal-box max-w-3xl relative" class:mb-0={bottom} on:cancel={cancelModal}>
      {#if showCloseButton}
        <button class="btn btn-sm btn-circle absolute right-2 top-2" aria-label={$t('close')} on:click={cancelModal}
          >✕</button
        >
      {/if}
      <slot {closing} />
      {#if $$slots.actions}
        <div class="modal-action justify-between">
          <div class="flex gap-4">
            <slot name="extraActions" />
          </div>
          <div class="flex gap-4">
            <slot name="actions" {closing} />
          </div>
        </div>
      {/if}
    </dialog>
  </div>
{/if}
