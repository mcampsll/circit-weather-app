<template>
  <v-app-bar elevation="0" color="surface" border="b">
    <v-app-bar-title class="clickable" @click="router.push('/')">
      <v-icon icon="mdi-weather-cloudy" color="primary" class="mr-2" />
      <span class="font-weight-bold">Circit</span>
      <span class="text-primary font-weight-bold"> Weather</span>
    </v-app-bar-title>

    <template #append>
      <v-menu v-if="auth.isLoggedIn" offset-y>
        <template #activator="{ props }">
          <div v-bind="props" class="user-chip mr-3 clickable">
            <v-avatar size="28"><v-img :src="auth.user?.avatar_url" /></v-avatar>
            <span class="user-name">{{ auth.user?.name || auth.user?.login }}</span>
            <v-icon icon="mdi-chevron-down" size="16" />
          </div>
        </template>
        <v-card rounded="lg" width="200" class="pa-2">
          <v-list-item
            prepend-icon="mdi-account-outline"
            title="My Profile"
            @click="router.push('/profile')"
          />
          <v-list-item
            prepend-icon="mdi-weather-cloudy"
            title="Weather"
            @click="router.push('/weather')"
          />
          <v-divider class="my-1" />
          <v-list-item
            prepend-icon="mdi-logout"
            title="Sign out"
            base-color="error"
            @click="signOut"
          />
        </v-card>
      </v-menu>

      <div v-else class="upsell mr-3">
        <span class="upsell-text">Authenticate with GitHub OAuth2</span>
        <v-btn
          variant="tonal"
          color="primary"
          rounded="pill"
          size="small"
          @click="router.push('/auth')"
        >
          <v-icon icon="mdi-github" class="mr-1" size="16" />Sign in
        </v-btn>
      </div>
    </template>
  </v-app-bar>
</template>

<script setup lang="ts">
  import { useRouter } from 'vue-router'
  import { useAuthStore } from '../stores/authStore'

  const router = useRouter()
  const auth = useAuthStore()

  function signOut() {
    auth.logout()
    router.push('/')
  }
</script>

<style scoped>
  .upsell {
    display: flex;
    align-items: center;
    gap: 12px;
    background: rgba(255, 255, 255, 0.04);
    border: 1px solid rgba(255, 255, 255, 0.08);
    border-radius: 12px;
    padding: 6px 6px 6px 12px;
  }
  .upsell-text {
    font-size: 0.78rem;
    color: rgba(255, 255, 255, 0.5);
    white-space: nowrap;
  }
  .user-chip {
    display: flex;
    align-items: center;
    gap: 8px;
    background: rgba(255, 255, 255, 0.04);
    border: 1px solid rgba(255, 255, 255, 0.08);
    border-radius: 24px;
    padding: 4px 12px 4px 4px;
    transition: background var(--transition-speed) ease;
  }
  .user-chip:hover {
    background: rgba(255, 255, 255, 0.08);
  }
  .user-name {
    font-size: 0.85rem;
    font-weight: 600;
    color: white;
  }
</style>
