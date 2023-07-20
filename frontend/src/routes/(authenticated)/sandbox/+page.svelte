<script lang="ts">
  import IconButton from '$lib/components/IconButton.svelte';
</script>
<div class="grid grid-cols-3 gap-2 ">
  <details class="collapse collapse-arrow bg-base-200">
    <summary class="collapse-title">Icon Buttons</summary>
    <div class="collapse-content">
      icon:
      <IconButton icon="i-mdi-cards-heart"/>
      loading:
      <IconButton loading icon="i-mdi-cards-heart"/>
    </div>
  </details>


  <details class="collapse collapse-arrow bg-base-200">
    <summary class="collapse-title">Buttons</summary>
    <div class="collapse-content">
      Normal:
      <button class="btn btn-primary">
        Click me
      </button>
      Loading:
      <button class="btn btn-primary">
        <span class="loading"></span>
        Click me
      </button>
    </div>
  </details>
</div>

