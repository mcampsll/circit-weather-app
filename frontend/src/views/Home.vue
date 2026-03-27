<template>
  <AppNavbar />
  <v-main>
    <div class="home-container">
      <div class="content text-center">
        <v-icon icon="mdi-weather-cloudy" size="80" color="primary" class="float mb-6" />
        <h1 class="text-h2 font-weight-bold mb-2">
          Circit <span class="text-primary">Weather</span>
        </h1>
        <p class="text-h6 text-medium-emphasis mb-12">
          Real-time weather, timezone &amp; astronomy data for cities worldwide
        </p>

        <v-row justify="center" dense style="align-items: stretch">
          <v-col cols="12" sm="5">
            <v-card
              rounded="xl"
              elevation="4"
              class="option-card pa-6 text-center clickable"
              @click="router.push('/weather')"
            >
              <v-icon icon="mdi-weather-partly-cloudy" size="48" color="primary" class="mb-3" />
              <div class="text-h6 font-weight-bold mb-2">Weather Explorer</div>
              <div class="card-desc text-body-2 text-medium-emphasis mb-4">
                Real-time weather, timezone and astronomy data for Dublin, Barcelona and Anchorage
              </div>
              <v-btn color="primary" rounded="lg" block>
                Explore Weather <v-icon icon="mdi-arrow-right" end />
              </v-btn>
            </v-card>
          </v-col>

          <v-col cols="12" sm="5">
            <v-card
              v-if="!auth.isLoggedIn"
              rounded="xl"
              elevation="4"
              class="option-card pa-6 text-center clickable"
              @click="router.push('/auth')"
            >
              <v-icon icon="mdi-shield-lock-outline" size="48" color="primary" class="mb-3" />
              <div class="text-h6 font-weight-bold mb-2">Sign in with GitHub</div>
              <div class="card-desc text-body-2 text-medium-emphasis mb-4">
                Authenticate securely using OAuth2 authorization code flow via GitHub
              </div>
              <v-btn color="primary" rounded="lg" block>
                Sign in <v-icon icon="mdi-github" end />
              </v-btn>
            </v-card>
            <v-card
              v-else
              rounded="xl"
              elevation="4"
              class="option-card pa-6 text-center"
              @click.stop
            >
              <v-avatar size="48" class="mb-3">
                <v-img :src="auth.user?.avatar_url" />
              </v-avatar>
              <div class="text-h6 font-weight-bold mb-2">
                {{ auth.user?.name || auth.user?.login }}
              </div>
              <div class="text-body-2 text-medium-emphasis mb-4">Signed in via GitHub OAuth2</div>
              <v-btn
                color="secondary"
                rounded="lg"
                block
                class="mb-2"
                @click="router.push('/profile')"
              >
                My Profile <v-icon icon="mdi-account" end />
              </v-btn>
              <v-btn color="error" rounded="lg" block variant="tonal" @click="signOut">
                Sign out <v-icon icon="mdi-logout" end />
              </v-btn>
            </v-card>
          </v-col>
        </v-row>
      </div>
    </div>
    <AppFooter />
  </v-main>
</template>

<script setup lang="ts">
  import { onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import { useAuthStore } from '../stores/authStore'
  import { useWeatherStore } from '../stores/weatherStore'
  import AppNavbar from '../components/AppNavbar.vue'
  import AppFooter from '../components/AppFooter.vue'

  const router = useRouter()
  const auth = useAuthStore()
  const weatherStore = useWeatherStore()

  onMounted(() => weatherStore.clearDetail())

  function signOut() {
    auth.logout()
    router.push('/')
  }
</script>

<style scoped>
  .home-container {
    min-height: calc(100vh - 96px);
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding: 2rem;
    background: radial-gradient(ellipse at top, #1a1a2e 0%, #0d1117 70%);
  }
  .content {
    max-width: 800px;
    width: 100%;
  }
  .option-card {
    transition: transform var(--transition-speed) ease;
    height: 100%;
  }
  .option-card:hover {
    transform: translateY(-6px);
  }
  .card-desc {
    min-height: 60px;
  }
</style>
