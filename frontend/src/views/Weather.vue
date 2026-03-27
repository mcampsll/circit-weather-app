<template>
  <AppNavbar />
  <v-main>
    <div class="page-wrapper">
      <!-- CITY SELECTOR -->
      <template v-if="!store.selectedCity">
        <div v-if="store.previewsLoading" class="center-state">
          <v-progress-circular indeterminate color="primary" size="56" width="3" />
          <div class="text-body-1 text-medium-emphasis mt-4">Loading cities...</div>
        </div>

        <div v-else-if="store.previewsError" class="center-state">
          <v-icon icon="mdi-weather-cloudy-alert" size="80" color="primary" class="float mb-4" />
          <div class="text-h5 font-weight-bold mb-2">Could not load cities</div>
          <div class="text-body-2 text-medium-emphasis mb-4">
            Check your connection and try again
          </div>
          <v-btn color="primary" rounded="pill" variant="tonal" @click="store.fetchCityPreviews()">
            <v-icon icon="mdi-refresh" class="mr-2" />Try again
          </v-btn>
        </div>

        <v-row v-else class="fade-in" :class="{ 'justify-center': store.cityPreviews.length < 3 }">
          <v-col v-for="p in store.cityPreviews" :key="p.city" cols="12" sm="6" md="4">
            <CitySelectorCard :city="p.city" :data="p.data" @select="store.fetchWeather($event)" />
          </v-col>
        </v-row>
      </template>

      <!-- DETAIL VIEW -->
      <template v-else>
        <div class="detail-header">
          <v-btn
            variant="text"
            color="primary"
            prepend-icon="mdi-arrow-left"
            @click="store.clearDetail()"
          >
            All Cities
          </v-btn>
        </div>

        <div v-if="store.loading" class="center-state">
          <v-progress-circular indeterminate color="primary" size="56" width="3" />
          <div class="text-body-1 text-medium-emphasis mt-4">Fetching weather data...</div>
        </div>

        <div v-else-if="store.error" class="center-state">
          <v-icon icon="mdi-weather-cloudy-alert" size="80" color="primary" class="float mb-4" />
          <div class="text-h5 font-weight-bold mb-2">Could not load weather</div>
          <div class="text-body-2 text-medium-emphasis mb-4">
            Check your connection and try again
          </div>
          <v-btn color="primary" rounded="pill" variant="tonal" @click="store.clearDetail()">
            <v-icon icon="mdi-arrow-left" class="mr-2" />Go back
          </v-btn>
        </div>

        <div v-else-if="store.weatherData" class="data-grid fade-in">
          <div class="left-col">
            <TimezonePanel :location="store.weatherData.current.location" style="flex: 1" />
            <AstronomyPanel :astro="store.weatherData.astronomy.astronomy.astro" style="flex: 2" />
          </div>
          <WeatherPanel :current="store.weatherData.current.current" />
        </div>
      </template>
    </div>
  </v-main>
  <AppFooter />
</template>

<script setup lang="ts">
  import { onMounted } from 'vue'
  import { useWeatherStore } from '../stores/weatherStore'
  import AppNavbar from '../components/AppNavbar.vue'
  import AppFooter from '../components/AppFooter.vue'
  import CitySelectorCard from '../components/CitySelectorCard.vue'
  import WeatherPanel from '../components/WeatherPanel.vue'
  import TimezonePanel from '../components/TimezonePanel.vue'
  import AstronomyPanel from '../components/AstronomyPanel.vue'

  const store = useWeatherStore()
  onMounted(() => store.fetchCityPreviews()) // previews are cached in store, no double fetch
</script>

<style scoped>
  .page-wrapper {
    min-height: calc(100vh - 112px);
    display: flex;
    flex-direction: column;
    padding: 20px 24px 0;
    max-width: 1400px;
    margin: 0 auto;
    width: 100%;
  }
  .center-state {
    flex: 1;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    text-align: center;
  }
  .detail-header {
    margin-bottom: 8px;
  }
  .data-grid {
    flex: 1;
    display: grid;
    grid-template-columns: 320px 1fr;
    gap: 16px;
    height: calc(100vh - 180px);
  }
  .left-col {
    display: flex;
    flex-direction: column;
    gap: 16px;
    min-height: 0;
  }

  @media (max-width: 1024px) {
    .data-grid {
      grid-template-columns: 280px 1fr;
    }
  }
  @media (max-width: 768px) {
    .page-wrapper {
      padding: 16px 16px 0;
    }
    .data-grid {
      grid-template-columns: 1fr;
      height: auto;
    }
  }
</style>
