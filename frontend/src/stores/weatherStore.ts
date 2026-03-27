import { defineStore } from 'pinia'
import axios from 'axios'
import type { CityCurrentResponse, AstronomyResponse } from '../types/weather'

const API_BASE = import.meta.env.VITE_API_URL ?? 'http://localhost:5000/api/weather'

export type City = string

export interface CityPreview {
  city: City
  data: CityCurrentResponse
}
export interface WeatherData {
  current: CityCurrentResponse
  astronomy: AstronomyResponse
}

export const useWeatherStore = defineStore('weather', {
  state: () => ({
    cityPreviews: [] as CityPreview[],
    previewsLoading: false,
    previewsError: null as string | null,
    selectedCity: null as City | null,
    weatherData: null as WeatherData | null,
    loading: false,
    error: null as string | null,
  }),
  actions: {
    async fetchCityPreviews() {
      if (this.cityPreviews.length) return // don't re-fetch if cached
      this.previewsLoading = true
      this.previewsError = null
      try {
        const { data } = await axios.get<CityPreview[]>(`${API_BASE}/cities`)
        this.cityPreviews = data
      } catch {
        this.previewsError = 'Failed to load cities. Please try again.'
      } finally {
        this.previewsLoading = false
      }
    },
    async fetchWeather(city: City) {
      this.loading = true
      this.error = null
      this.selectedCity = city
      this.weatherData = null
      try {
        const { data } = await axios.get<WeatherData>(`${API_BASE}/all/${city}`)
        this.weatherData = data
      } catch {
        this.error = 'Failed to fetch weather data. Please try again.'
      } finally {
        this.loading = false
      }
    },
    clearDetail() {
      this.selectedCity = null
      this.weatherData = null
      this.error = null
    },
  },
})
