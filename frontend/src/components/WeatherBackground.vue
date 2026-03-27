<template>
  <div class="weather-bg" :class="condition">
    <div class="gradient-layer" />

    <template v-if="condition === 'night'">
      <div v-for="s in stars" :key="s.id" class="star" :style="s.style" />
    </template>
    <template v-if="condition === 'rain'">
      <div v-for="d in drops" :key="d.id" class="drop" :style="d.style" />
    </template>
    <template v-if="condition === 'day'">
      <div class="sun">
        <div class="sun-core" />
        <div v-for="i in 8" :key="i" class="ray" :style="{ transform: `rotate(${i * 45}deg)` }" />
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
  import { computed, ref, onMounted } from 'vue'

  const props = defineProps<{ isDay: number; conditionCode: number }>()

  const RAINY = new Set([1063, 1180, 1183, 1186, 1189, 1192, 1195, 1240, 1243, 1246])

  const condition = computed(() =>
    !props.isDay ? 'night' : RAINY.has(props.conditionCode) ? 'rain' : 'day'
  )

  interface P {
    id: number
    style: Record<string, string>
  }
  const stars = ref<P[]>([])
  const drops = ref<P[]>([])

  onMounted(() => {
    stars.value = Array.from({ length: 20 }, (_, i) => ({
      id: i,
      style: {
        width: `${Math.random() * 2 + 1}px`,
        height: `${Math.random() * 2 + 1}px`,
        top: `${Math.random() * 100}%`,
        left: `${Math.random() * 100}%`,
        animationDelay: `${Math.random() * 3}s`,
        animationDuration: `${Math.random() * 2 + 2}s`,
      },
    }))
    drops.value = Array.from({ length: 12 }, (_, i) => ({
      id: i,
      style: {
        left: `${(i / 12) * 100 + Math.random() * 8}%`,
        animationDelay: `${Math.random() * 2}s`,
        animationDuration: `${Math.random() * 0.5 + 0.8}s`,
      },
    }))
  })
</script>

<style scoped>
  .weather-bg {
    position: absolute;
    inset: 0;
    overflow: hidden;
    border-radius: inherit;
    pointer-events: none;
  }
  .gradient-layer {
    position: absolute;
    inset: 0;
  }
  .night .gradient-layer {
    background: linear-gradient(160deg, #0f0c29 0%, #1a1a4e 50%, #24243e 100%);
  }
  .rain .gradient-layer {
    background: linear-gradient(160deg, #1c2f45 0%, #1e3a5a 100%);
  }
  .day .gradient-layer {
    background: linear-gradient(160deg, #1a2a3a 0%, #1e3a5a 50%, #2a4a6a 100%);
  }

  .star {
    position: absolute;
    background: white;
    border-radius: 50%;
    animation: twinkle var(--dur, 2s) ease-in-out infinite;
  }
  @keyframes twinkle {
    0%,
    100% {
      opacity: 0.2;
      transform: scale(1);
    }
    50% {
      opacity: 1;
      transform: scale(1.4);
    }
  }

  .drop {
    position: absolute;
    top: -20px;
    width: 1.5px;
    height: 18px;
    background: linear-gradient(to bottom, transparent, rgba(255, 255, 255, 0.5));
    border-radius: 2px;
    animation: fall linear infinite;
  }
  @keyframes fall {
    0% {
      top: -20px;
      opacity: 0;
    }
    10% {
      opacity: 1;
    }
    90% {
      opacity: 1;
    }
    100% {
      top: 110%;
      opacity: 0;
    }
  }

  .sun {
    position: absolute;
    top: 15%;
    right: 10%;
    width: 60px;
    height: 60px;
    display: flex;
    align-items: center;
    justify-content: center;
    animation: rotateSun 20s linear infinite;
  }
  .sun-core {
    position: absolute;
    width: 28px;
    height: 28px;
    background: rgba(255, 200, 50, 0.3);
    border-radius: 50%;
    box-shadow: 0 0 20px rgba(255, 200, 50, 0.2);
  }
  .ray {
    position: absolute;
    width: 2px;
    height: 14px;
    background: rgba(255, 200, 50, 0.25);
    border-radius: 2px;
    top: 0;
    left: 50%;
    transform-origin: bottom center;
  }
  @keyframes rotateSun {
    from {
      transform: rotate(0deg);
    }
    to {
      transform: rotate(360deg);
    }
  }
</style>
