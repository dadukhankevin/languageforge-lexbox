<script lang="ts">
  import Modal, { DialogResponse } from './Modal.svelte';
  import t from '$lib/i18n';
  import FormError from '$lib/forms/FormError.svelte';
  import type { ErrorMessage } from '$lib/forms';

  export let entityName: string;
  export let isRemoveDialog = false;
  let modal: Modal;
  let error: ErrorMessage = undefined;

  export async function prompt(deleteCallback: () => Promise<ErrorMessage>): Promise<boolean> {
    if ((await modal.openModal()) === DialogResponse.Cancel) {
      error = undefined;
      return false;
    }
    error = await deleteCallback();
    if (error) {
      return prompt(deleteCallback);
    }
    modal.close();
    error = undefined;
    return true;
  }
</script>

<Modal bind:this={modal} showCloseButton={false}>
  <h2 class="text-xl mb-2">
    {#if isRemoveDialog}
      {$t('delete_modal.remove', { entityName })}
    {:else}
      {$t('delete_modal.delete', { entityName })}
    {/if}
  </h2>
  <slot />
  <FormError {error} right />
  <svelte:fragment slot="actions" let:closing>
    <button class="btn btn-error" class:loading={closing} on:click={() => modal.submitModal()}>
      <span class="i-mdi-trash text-2xl mr-2" />
      {#if isRemoveDialog}
        {$t('delete_modal.remove', { entityName })}
      {:else}
        {$t('delete_modal.delete', { entityName })}
      {/if}
    </button>
    <button class="btn" disabled={closing} on:click={() => modal.cancelModal()}>
      {#if isRemoveDialog}
        {$t('delete_modal.dont_remove')}
      {:else}
        {$t('delete_modal.dont_delete')}
      {/if}
    </button>
  </svelte:fragment>
</Modal>
