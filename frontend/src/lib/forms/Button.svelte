<script lang="ts">
  export let loading = false;
</script>

<!-- https://daisyui.com/components/button -->
<button on:click class="btn btn-primary">
  <slot />
  <span class:loading></span>
</button>
