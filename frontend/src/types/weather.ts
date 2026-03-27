export interface WeatherCondition {
  text: string
  icon: string
  code: number
}

export interface WeatherCurrent {
  temp_c: number
  feelslike_c: number
  humidity: number
  wind_kph: number
  wind_dir: string
  pressure_mb: number
  vis_km: number
  uv: number
  precip_mm: number
  cloud: number
  gust_kph: number
  dewpoint_c: number
  heatindex_c: number
  windchill_c: number
  is_day: number
  condition: WeatherCondition
}

export interface Location {
  name: string
  country: string
  region: string
  tz_id: string
  localtime: string
}

export interface AstroData {
  sunrise: string
  sunset: string
  moonrise: string
  moonset: string
  moon_phase: string
  moon_illumination: number
}

export interface CityCurrentResponse {
  location: Location
  current: WeatherCurrent
}

export interface AstronomyResponse {
  location: Location
  astronomy: {
    astro: AstroData
  }
}
