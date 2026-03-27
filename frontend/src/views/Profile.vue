<template>
  <AppNavbar />
  <v-main>
    <div class="auth-wrapper">
      <div class="auth-state">
        <v-avatar size="100" class="mb-4">
          <v-img :src="auth.user!.avatar_url" :alt="auth.user!.login" />
        </v-avatar>
        <div class="text-h4 font-weight-bold mb-1">{{ auth.user!.name || auth.user!.login }}</div>
        <div class="text-body-1 text-medium-emphasis mb-1">@{{ auth.user!.login }}</div>
        <div v-if="auth.user!.bio" class="text-body-2 text-medium-emphasis mb-4 text-center bio">
          {{ auth.user!.bio }}
        </div>
        <div class="user-stats mb-6">
          <div v-for="s in userStats" :key="s.label" class="stat">
            <div class="text-h6 font-weight-bold">{{ s.value }}</div>
            <div class="label">{{ s.label }}</div>
          </div>
        </div>
        <div class="d-flex flex-column align-center" style="gap: 12px">
          <v-btn color="error" rounded="pill" variant="tonal" min-width="200" @click="signOut">
            <v-icon icon="mdi-logout" class="mr-2" />Sign out
          </v-btn>
        </div>
      </div>
    </div>
  </v-main>
  <AppFooter />
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  import { useRouter } from 'vue-router'
  import { useAuthStore } from '../stores/authStore'
  import AppNavbar from '../components/AppNavbar.vue'
  import AppFooter from '../components/AppFooter.vue'

  const router = useRouter()
  const auth = useAuthStore()

  const userStats = computed(() => [
    { label: 'Repos', value: auth.user!.public_repos },
    { label: 'Followers', value: auth.user!.followers },
    { label: 'Following', value: auth.user!.following },
  ])

  function signOut() {
    auth.logout()
    router.push('/')
  }
</script>

<style scoped>
  .auth-wrapper {
    min-height: calc(100vh - 112px);
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 2rem;
  }
  .auth-state {
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;
  }
  .bio {
    max-width: 400px;
  }
  .user-stats {
    display: flex;
    gap: 48px;
  }
  .stat {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 4px;
  }
</style>
