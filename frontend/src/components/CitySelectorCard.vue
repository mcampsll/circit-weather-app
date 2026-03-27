<template>
  <div class="city-card clickable" @click="$emit('select', city)">
    <div class="card-hero">
      <WeatherBackground
        :is-day="data.current.is_day"
        :condition-code="data.current.condition.code"
      />

      <div class="card-top">
        <div>
          <div class="city-name">{{ data.location.name }}</div>
          <div class="label mt-1">{{ data.location.country }}</div>
        </div>
        <div class="time-badge">{{ localTime }}</div>
      </div>

      <div class="card-center">
        <img :src="`https:${data.current.condition.icon}`" class="weather-icon" />
        <div class="big-temp">{{ data.current.temp_c }}°C</div>
        <div class="label mt-1">{{ data.current.condition.text }}</div>
        <div class="day-badge mt-2">
          <v-icon
            :icon="data.current.is_day ? 'mdi-weather-sunny' : 'mdi-weather-night'"
            size="12"
            class="mr-1"
          />
          {{ data.current.is_day ? 'Day' : 'Night' }}
        </div>
      </div>

      <div class="card-bottom">
        <div class="card-stats">
          <div v-for="stat in cardStats" :key="stat.label" class="stat-item">
            <v-icon :icon="stat.icon" size="16" color="white" />
            <div class="value-large mt-1">{{ stat.value }}</div>
            <div class="label">{{ stat.label }}</div>
          </div>
        </div>
        <div class="explore-hint">
          Tap to explore <v-icon icon="mdi-arrow-right" size="13" class="ml-1" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  import type { City, CityPreview } from '../stores/weatherStore'
  import WeatherBackground from './WeatherBackground.vue'

  const props = defineProps<{ city: City; data: CityPreview['data'] }>()
  defineEmits<{ select: [city: City] }>()

  const cardStats = computed(() => [
    { icon: 'mdi-water-percent', value: `${props.data.current.humidity}%`, label: 'Humidity' },
    { icon: 'mdi-weather-windy', value: `${props.data.current.wind_kph} kph`, label: 'Wind' },
    { icon: 'mdi-thermometer', value: `${props.data.current.feelslike_c}°C`, label: 'Feels like' },
  ])

  const localTime = computed(() => props.data.location.localtime?.split(' ')[1] ?? '—')
</script>

<style scoped>
  .city-card {
    border-radius: 24px;
    overflow: hidden;
    height: 100%;
    transition:
      transform 0.3s cubic-bezier(0.34, 1.56, 0.64, 1),
      box-shadow 0.3s ease;
    box-shadow: 0 4px 24px rgba(0, 0, 0, 0.2);
  }
  .city-card:hover {
    transform: translateY(-8px) scale(1.01);
    box-shadow: 0 24px 48px rgba(0, 0, 0, 0.4);
  }

  .card-hero {
    padding: 24px;
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    position: relative;
    overflow: hidden;
  }
  .card-top {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    position: relative;
    z-index: 1;
  }
  .card-center {
    display: flex;
    flex-direction: column;
    align-items: center;
    position: relative;
    z-index: 1;
    flex: 1;
    justify-content: center;
    padding: 12px 0;
  }
  .card-bottom {
    position: relative;
    z-index: 1;
  }

  .city-name {
    font-size: 1.8rem;
    font-weight: 800;
    color: white;
    letter-spacing: -0.5px;
    line-height: 1;
  }
  .time-badge {
    font-size: 0.85rem;
    font-weight: 600;
    color: rgba(255, 255, 255, 0.7);
    background: rgba(255, 255, 255, 0.1);
    padding: 4px 12px;
    border-radius: 20px;
    white-space: nowrap;
    border: 1px solid rgba(255, 255, 255, 0.15);
  }
  .weather-icon {
    width: 72px;
    filter: drop-shadow(0 4px 16px rgba(0, 0, 0, 0.4));
  }
  .big-temp {
    font-size: 2.8rem;
    font-weight: 900;
    color: white;
    line-height: 1;
    margin-top: 6px;
    letter-spacing: -2px;
  }
  .day-badge {
    display: inline-flex;
    align-items: center;
    padding: 3px 10px;
    border-radius: 20px;
    font-size: 0.72rem;
    font-weight: 600;
    background: rgba(255, 255, 255, 0.15);
    color: white;
    border: 1px solid rgba(255, 255, 255, 0.2);
    backdrop-filter: blur(4px);
  }

  .card-stats {
    display: flex;
    align-items: center;
    justify-content: space-around;
    padding: 12px 0;
    border-top: 1px solid rgba(255, 255, 255, 0.12);
    border-bottom: 1px solid rgba(255, 255, 255, 0.12);
    margin-bottom: 10px;
  }
  .stat-item {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 2px;
  }
  .explore-hint {
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 0.8rem;
    font-weight: 600;
    color: rgba(255, 255, 255, 0.4);
  }

  @media (max-width: 768px) {
    .city-name {
      font-size: 1.5rem;
    }
    .big-temp {
      font-size: 2.4rem;
    }
    .weather-icon {
      width: 60px;
    }
  }
</style>
