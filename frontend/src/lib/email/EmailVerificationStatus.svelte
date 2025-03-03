<script lang="ts">
  import t from '$lib/i18n';
  import { slide } from 'svelte/transition';
  import type { LexAuthUser } from '$lib/user';
  import type { EmailResult } from '.';

  export let user: LexAuthUser;
  export let emailResult: EmailResult | null = null;
  export let requestedEmail: string | undefined = undefined;

  let sendingVerificationEmail = false;
  let sentVerificationEmail = false;

  async function sendVerificationEmail(): Promise<void> {
    sendingVerificationEmail = true;
    try {
      const result = await fetch('/api/user/sendVerificationEmail', { method: 'POST' });
      if (!result.ok) throw Error(`Failed to send verification email. ${result.status}: ${result.statusText}.`);
      sentVerificationEmail = true;
    } finally {
      sendingVerificationEmail = false;
    }
  }
</script>

{#if emailResult}
  <div class="alert alert-success" transition:slide|local>
    {#if emailResult == 'verifiedEmail'}
      <span>{$t('account_settings.verify_email.verify_success')}</span>
    {:else}
      <span>{$t('account_settings.verify_email.change_success')}</span>
    {/if}
    <span class="i-mdi-check-circle-outline" />
    <a class="btn" href="/">{$t('account_settings.verify_email.go_to_projects')}</a>
  </div>
{:else if requestedEmail}
  <div class="alert alert-info" transition:slide|local>
    <div>
      <span>{$t('account_settings.verify_email.you_have_mail')}</span>
      <span>{$t('account_settings.verify_email.verify_to_change', { requestedEmail })}</span>
    </div>
    <span class="i-mdi-email-heart-outline" />
  </div>
{:else if !user?.emailVerified}
  {#if sentVerificationEmail}
    <div class="alert alert-info" transition:slide|local>
      <div>
        <span>{$t('account_settings.verify_email.you_have_mail')}</span>
        <span>{$t('account_settings.verify_email.check_inbox')}</span>
      </div>
      <span class="i-mdi-email-heart-outline" />
    </div>
  {:else}
    <div class="alert alert-warning" transition:slide|local>
      <span>{$t('account_settings.verify_email.please_verify')}</span>
      <button class="btn" class:loading={sendingVerificationEmail} on:click={sendVerificationEmail}>
        {$t('account_settings.verify_email.resend')}</button
      >
    </div>
  {/if}
{/if}

<style lang="postcss">
  .alert {
    @apply text-base;

    & > span[class*='i-mdi'] {
      flex: 0 0 40px;
      @apply text-5xl;
    }

    & > div {
      @apply flex-col items-start;
      & > span:first-child {
        @apply font-bold;
      }
    }
  }
</style>
