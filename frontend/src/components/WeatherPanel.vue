<template>
  <div class="weather-panel">
    <WeatherBackground :is-day="current.is_day" :condition-code="current.condition.code" />

    <div class="panel-content">
      <div class="top-section">
        <div class="main-info">
          <img :src="`https:${current.condition.icon}`" width="80" class="weather-icon" />
          <div class="temp">{{ current.temp_c }}°C</div>
          <div class="condition">{{ current.condition.text }}</div>
          <div class="day-badge mt-2">
            <v-icon
              :icon="current.is_day ? 'mdi-weather-sunny' : 'mdi-weather-night'"
              size="14"
              class="mr-1"
            />
            {{ current.is_day ? 'Day' : 'Night' }}
          </div>
        </div>

        <div class="extras">
          <div v-for="e in extras" :key="e.label" class="extra-item">
            <div class="extra-icon-label">
              <div class="extra-dot" />
              <div class="extra-label">{{ e.label }}</div>
            </div>
            <div class="extra-value">{{ e.value }}</div>
          </div>
        </div>
      </div>

      <div class="stats-grid">
        <div v-for="s in stats" :key="s.label" class="stat-tile">
          <v-icon :icon="s.icon" :color="s.color" size="20" />
          <div class="label mt-1">{{ s.label }}</div>
          <div class="stat-value">{{ s.value }}</div>
          <div v-if="s.sub" class="label">{{ s.sub }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  import type { WeatherCurrent } from '../types/weather'
  import WeatherBackground from './WeatherBackground.vue'

  const props = defineProps<{ current: WeatherCurrent }>()
  const c = computed(() => props.current)

  const stats = computed(() => [
    {
      icon: 'mdi-thermometer',
      color: 'error',
      label: 'Feels like',
      value: `${c.value.feelslike_c}°C`,
      sub: null,
    },
    {
      icon: 'mdi-water-percent',
      color: 'info',
      label: 'Humidity',
      value: `${c.value.humidity}%`,
      sub: null,
    },
    {
      icon: 'mdi-weather-windy',
      color: 'primary',
      label: 'Wind',
      value: `${c.value.wind_kph} kph`,
      sub: c.value.wind_dir,
    },
    {
      icon: 'mdi-gauge',
      color: 'secondary',
      label: 'Pressure',
      value: `${c.value.pressure_mb} mb`,
      sub: null,
    },
    {
      icon: 'mdi-eye',
      color: 'success',
      label: 'Visibility',
      value: `${c.value.vis_km} km`,
      sub: null,
    },
    {
      icon: 'mdi-weather-sunny-alert',
      color: 'warning',
      label: 'UV Index',
      value: `${c.value.uv}`,
      sub: null,
    },
    {
      icon: 'mdi-weather-rainy',
      color: 'info',
      label: 'Precipitation',
      value: `${c.value.precip_mm} mm`,
      sub: null,
    },
    {
      icon: 'mdi-cloud',
      color: 'primary',
      label: 'Cloud cover',
      value: `${c.value.cloud}%`,
      sub: null,
    },
    {
      icon: 'mdi-weather-windy-variant',
      color: 'secondary',
      label: 'Gust',
      value: `${c.value.gust_kph} kph`,
      sub: null,
    },
  ])

  const extras = computed(() => [
    { label: 'Dew point', value: `${c.value.dewpoint_c}°C` },
    { label: 'Heat index', value: `${c.value.heatindex_c}°C` },
    { label: 'Wind chill', value: `${c.value.windchill_c}°C` },
  ])
</script>

<style scoped>
  .weather-panel {
    border-radius: 20px;
    height: 100%;
    position: relative;
    overflow: hidden;
  }
  .panel-content {
    position: relative;
    z-index: 1;
    height: 100%;
    display: flex;
    flex-direction: column;
    gap: 20px;
    padding: 28px;
  }

  .top-section {
    display: flex;
    gap: 32px;
    align-items: center;
    justify-content: center;
    padding-bottom: 20px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  }
  .main-info {
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;
    min-width: 220px;
  }
  .weather-icon {
    filter: drop-shadow(0 4px 12px rgba(0, 0, 0, 0.3));
  }
  .temp {
    font-size: 3.8rem;
    font-weight: 900;
    color: white;
    line-height: 1;
    margin-top: 6px;
    letter-spacing: -2px;
  }
  .condition {
    font-size: 1rem;
    color: rgba(255, 255, 255, 0.7);
    margin-top: 4px;
  }
  .day-badge {
    display: inline-flex;
    align-items: center;
    padding: 4px 10px;
    border-radius: 20px;
    font-size: 0.78rem;
    font-weight: 600;
    background: rgba(255, 255, 255, 0.2);
    color: white;
    border: 1px solid rgba(255, 255, 255, 0.3);
    backdrop-filter: blur(4px);
    margin-top: 8px;
  }

  .extras {
    flex: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    gap: 12px;
    padding-left: 24px;
    border-left: 1px solid rgba(255, 255, 255, 0.1);
    max-width: 280px;
  }
  .extra-item {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 10px 14px;
    border-radius: 12px;
    background: rgba(255, 255, 255, 0.06);
    border: 1px solid rgba(255, 255, 255, 0.08);
  }
  .extra-icon-label {
    display: flex;
    align-items: center;
    gap: 8px;
  }
  .extra-dot {
    width: 6px;
    height: 6px;
    border-radius: 50%;
    background: rgba(255, 255, 255, 0.4);
  }
  .extra-label {
    font-size: 0.72rem;
    font-weight: 500;
    letter-spacing: 0.08em;
    text-transform: uppercase;
    color: rgba(255, 255, 255, 0.4);
  }
  .extra-value {
    font-size: 1.2rem;
    font-weight: 700;
    color: white;
  }

  .stats-grid {
    flex: 1;
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 8px;
    align-content: stretch;
  }
  .stat-value {
    font-size: 0.9rem;
    font-weight: 700;
    color: white;
    margin-top: 2px;
  }

  @media (max-width: 768px) {
    .top-section {
      flex-direction: column;
      align-items: center;
      border-bottom: none;
      padding-bottom: 0;
    }
    .extras {
      width: 100%;
      max-width: 100%;
      border-left: none;
      border-top: 1px solid rgba(255, 255, 255, 0.1);
      padding-left: 0;
      padding-top: 16px;
    }
  }
</style>
