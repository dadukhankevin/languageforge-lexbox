<script lang="ts">
  // valid icons https://icon-sets.iconify.design/mdi/
  //syntax i-mdi-<icon-name>
  export let icon: `i-mdi-${string}`;
  export let disabled = false;
  export let loading = false;
  export let ghost = false;
</script>

<button {disabled} on:click class="btn btn-square {ghost ? 'btn-ghost' : 'btn-outline'}">
  {#if !loading}
    <span class="{icon} text-lg" />
  {:else}
    <span class="{icon} text-lg" />
    <span class:loading></span>
  {/if}
</button>
