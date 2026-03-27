<template>
  <div class="glass-card pa-5 timezone-panel">
    <div class="city-name">{{ location.name }}</div>
    <div class="label mb-4">{{ location.country }}</div>
    <div class="time">{{ localTime }}</div>
    <div class="label mb-4">{{ localDate }}</div>
    <v-divider class="mb-3" style="border-color: rgba(255, 255, 255, 0.1)" />
    <div class="label">Timezone</div>
    <div class="value-large">{{ location.tz_id }}</div>
  </div>
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  import type { Location } from '../types/weather'

  const props = defineProps<{ location: Location }>()

  const [localDate, localTime] = computed(() =>
    (props.location.localtime ?? '— —').split(' ')
  ).value
</script>

<style scoped>
  .timezone-panel {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }
  .city-name {
    font-size: 2.4rem;
    font-weight: 800;
    color: white;
    letter-spacing: -1px;
    line-height: 1;
    margin-bottom: 4px;
  }
  .time {
    font-size: 2.8rem;
    font-weight: 800;
    letter-spacing: -1px;
    line-height: 1;
    margin-bottom: 4px;
  }
</style>
