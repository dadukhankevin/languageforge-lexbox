<script lang="ts">
  import FormField from './FormField.svelte';
  import { randomFieldId } from './utils';

  export let label: string;
  export let value: string;
  export let id = randomFieldId();
  export let autofocus = false;
  export let error: string | string[] | undefined = undefined;
</script>

<FormField {id} {label} {error} {autofocus}>
  <!-- svelte-ignore a11y-autofocus -->
  <select bind:value {id} class="select select-bordered" {autofocus}>
    <slot />
  </select>
</FormField>
