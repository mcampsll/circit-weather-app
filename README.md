# Circit Weather App

> Circit technical challenge: full-stack weather app built with Vue 3 and .NET 8, including a custom GitHub OAuth2 implementation. I really appreciated the opportunity and put significant effort into making it robust and well-structured. I hope you enjoy it.

## Tech Stack

**Frontend:** Vue 3, TypeScript, Vuetify 3, Pinia, Vue Router, Vite  
**Backend:** .NET 8, C#, ASP.NET Core Web API  
**Infrastructure:** Docker, Docker Compose  
**Auth:** GitHub OAuth2 (authorization code flow, no SDK)  
**API:** WeatherAPI.com via RapidAPI

## Features

- Real-time weather data for Dublin, Barcelona and Anchorage
- Current conditions: temperature, humidity, wind, pressure, UV index, visibility, precipitation and more
- Timezone and local time per city
- Astronomy data: sunrise, sunset, moon phase and illumination
- Dynamic weather backgrounds (rain, night, sun) with animated particles
- GitHub OAuth2 authentication — pure authorization code flow without any provider SDK
- GitHub user profile display after authentication
- Responsive design across all screen sizes
- .NET backend proxies all API calls — no keys exposed to the frontend

## Architecture
```
circit-weather-app/
├── backend/               # .NET 8 Web API
│   └── Controllers/
│       ├── WeatherController.cs   # Weather API proxy
│       └── AuthController.cs      # GitHub OAuth2 flow
├── frontend/              # Vue 3 + Vuetify
│   └── src/
│       ├── views/         # Home, Weather, Auth
│       ├── components/    # Reusable UI components
│       ├── stores/        # Pinia state management
│       └── types/         # TypeScript interfaces
├── backend.tests/         # xUnit tests
└── docker-compose.yml
```

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 22+
- Docker (optional)

### Environment Variables

Create a `.env` file in the root of the project with the following structure:
```env
GITHUB_CLIENT_ID=your_github_client_id
GITHUB_CLIENT_SECRET=your_github_client_secret
RAPIDAPI_KEY=your_rapidapi_key
```

To get a GitHub Client ID and Secret, create an OAuth App at `https://github.com/settings/developers` with:
- **Homepage URL:** `http://localhost:5173`
- **Callback URL:** `http://localhost:5000/auth/callback`

### Run with Docker (recommended)
```bash
cp .env.example .env   # fill in your values
docker-compose up --build
```

App will be available at `http://localhost:5173`

### Run Manually

**Backend:**
```bash
cd backend
dotnet run
```

**Frontend:**
```bash
cd frontend
npm install
npm run dev
```

## OAuth2 Flow

Authentication is implemented manually without any GitHub SDK:

1. User clicks "Continue with GitHub"
2. Backend redirects to `github.com/login/oauth/authorize`
3. GitHub redirects back to `/auth/callback` with a `code`
4. Backend exchanges `code` for `access_token` via `github.com/login/oauth/access_token`
5. Backend uses token to fetch user data from `api.github.com/user`
6. User data is returned to the frontend

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/weather/cities` | Current weather for all available cities |
| GET | `/api/weather/all/{city}` | Full weather data for a city |
| GET | `/auth/login` | Initiates GitHub OAuth2 flow |
| GET | `/auth/callback` | GitHub OAuth2 callback |

## Tests

Unit tests for the backend controllers using xUnit and Moq.
```bash
cd backend.tests
dotnet test
```

Tests cover:
- `WeatherController` — city validation, successful responses and error handling
- `AuthController` — OAuth2 callback with valid code, missing code and token exchange failure

## Adding More Cities

To add a new city, only one change is needed in the backend:
```csharp
private static readonly HashSet<string> AllowedCities = new(StringComparer.OrdinalIgnoreCase)
{
    "dublin", "barcelona", "anchorage", "london" // add here
};
```

The frontend dynamically renders whatever cities the `/api/weather/cities` endpoint returns — no frontend changes required.
