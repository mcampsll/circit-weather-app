<template>
  <AppNavbar />
  <v-main>
    <div class="auth-wrapper">
      <div v-if="loading" class="auth-state">
        <v-progress-circular indeterminate color="primary" size="56" width="3" />
        <div class="text-body-1 text-medium-emphasis mt-4">Authenticating...</div>
      </div>

      <div v-else-if="error" class="auth-state">
        <v-icon icon="mdi-alert-circle-outline" size="80" color="error" class="mb-4" />
        <div class="text-h5 font-weight-bold mb-2">Authentication failed</div>
        <div class="text-body-2 text-medium-emphasis mb-4">{{ error }}</div>
        <v-btn color="primary" rounded="pill" variant="tonal" @click="router.push('/')">
          <v-icon icon="mdi-arrow-left" class="mr-2" />Go back
        </v-btn>
      </div>

      <div v-else class="auth-state">
        <v-icon icon="mdi-github" size="80" color="primary" class="float mb-6" />
        <div class="text-h4 font-weight-bold mb-2">Sign in with GitHub</div>
        <div class="text-body-1 text-medium-emphasis mb-2">Authenticate securely using OAuth2</div>
        <div class="text-body-2 text-medium-emphasis mb-8 bio">
          No SDK used — pure OAuth2 authorization code flow implemented from scratch in C# and Vue
        </div>
        <v-btn color="primary" rounded="pill" size="large" href="http://localhost:5000/auth/login">
          <v-icon icon="mdi-github" class="mr-2" />Continue with GitHub
        </v-btn>
      </div>
    </div>
  </v-main>
  <AppFooter />
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  import { useAuthStore } from '../stores/authStore'
  import AppNavbar from '../components/AppNavbar.vue'
  import AppFooter from '../components/AppFooter.vue'

  const router = useRouter()
  const route = useRoute()
  const auth = useAuthStore()

  const loading = ref(false)
  const error = ref<string | null>(null)

  onMounted(() => {
    // If already logged in, go straight to profile
    if (auth.isLoggedIn) {
      router.replace({ name: 'profile' })
      return
    }

    const userParam = route.query.user as string
    const errorParam = route.query.error as string

    if (errorParam) {
      error.value = 'Failed to authenticate with GitHub. Please try again.'
      return
    }
    if (userParam) {
      try {
        auth.setUser(JSON.parse(decodeURIComponent(userParam)))
        router.replace({ name: 'profile' })
      } catch {
        error.value = 'Failed to parse user data.'
      }
    }
  })
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
</style>
