<template>
  <div class="glass-card pa-5" style="flex: 2; display: flex; flex-direction: column">
    <div class="d-flex align-center mb-4">
      <v-icon icon="mdi-telescope" color="primary" class="mr-2" />
      <span class="text-subtitle-2 font-weight-bold text-uppercase" style="letter-spacing: 0.08em"
        >Astronomy</span
      >
    </div>

    <div class="astro-grid mb-3">
      <div v-for="item in astroItems" :key="item.label" class="stat-tile">
        <v-icon :icon="item.icon" :color="item.color" size="20" />
        <div class="label mt-1">{{ item.label }}</div>
        <div class="value-large">{{ item.value }}</div>
      </div>
    </div>

    <div class="stat-tile" style="flex: 1">
      <div class="d-flex align-center justify-space-between w-100">
        <div>
          <div class="label">Moon Phase</div>
          <div class="value-large mt-1">{{ astro.moon_phase }}</div>
        </div>
        <v-icon icon="mdi-moon-full" color="secondary" size="32" />
      </div>
      <v-progress-linear
        :model-value="astro.moon_illumination"
        color="secondary"
        rounded
        height="4"
        class="mt-3"
      />
      <div class="label mt-2">{{ astro.moon_illumination }}% illuminated</div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  import type { AstroData } from '../types/weather'

  const props = defineProps<{ astro: AstroData }>()

  const astroItems = computed(() => [
    { icon: 'mdi-weather-sunny', color: 'warning', label: 'Sunrise', value: props.astro.sunrise },
    {
      icon: 'mdi-weather-sunset',
      color: 'deep-orange',
      label: 'Sunset',
      value: props.astro.sunset,
    },
    {
      icon: 'mdi-moon-waning-crescent',
      color: 'secondary',
      label: 'Moonrise',
      value: props.astro.moonrise,
    },
    {
      icon: 'mdi-moon-waning-gibbous',
      color: 'secondary',
      label: 'Moonset',
      value: props.astro.moonset,
    },
  ])
</script>

<style scoped>
  .astro-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 10px;
  }
</style>
