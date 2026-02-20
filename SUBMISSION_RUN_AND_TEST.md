# Lemax Hotel Search - Run And Test Guide

## Prerequisites

- .NET 8 SDK installed
- OS: Windows, Linux, or macOS

Check SDK:

```bash
dotnet --version
```

## Project Structure

- `src/Lemax.HotelSearch.Api` - Web API (controllers, validation, middleware, Swagger)
- `src/Lemax.HotelSearch.Domain` - domain entities and value objects
- `src/Lemax.HotelSearch.Service` - business logic, search, scoring, DTO/read models
- `src/Lemax.HotelSearch.Infrastructure` - in-memory repositories and preseed (countries + cities)
- `tests/Lemax.HotelSearch.Test` - unit + integration tests

## Restore, Build, Test

From solution root (`/mnt/d/Lemax/LemaxHotelSearch`):

```bash
dotnet restore
dotnet build
dotnet test
```

## Run API

```bash
dotnet run --project src/Lemax.HotelSearch.Api
```

Swagger UI:

- `https://localhost:5001/swagger` or port shown in console

Health check:

- `GET /health`

## Seed Data

Outside tests, application pre-seeds only:

- countries
- cities

Hotels are not pre-seeded outside tests.

## Suggested Manual Smoke Test In Swagger

1. `GET /health` -> `200`
2. `GET /api/countries` -> returns seeded countries
3. `GET /api/cities` -> returns seeded cities
4. `POST /api/hotels` with valid `cityId` -> `201`
5. `GET /api/hotels/{id}` -> `200` and includes:
   - `countryId`, `countryName`
   - `cityId`, `cityName`
6. `GET /api/hotels/search/all?...` -> non-paged results
7. `GET /api/hotels/search?...` -> paged results
8. `POST /api/hotels` with invalid `cityId` -> `400` (`LogDetails`)

## Example Requests

Create hotel:

```json
{
  "name": "Hotel Example",
  "price": 120,
  "cityId": "00000000-0000-0000-0000-000000000000",
  "latitude": 45.815,
  "longitude": 15.981
}
```

Paged search:

`GET /api/hotels/search?searchLatitude=45.815&searchLongitude=15.981&radiusKm=20&sortBy=relevance&order=asc&page=1&pageSize=20`

Non-paged search:

`GET /api/hotels/search/all?searchLatitude=45.815&searchLongitude=15.981&sortBy=relevance&order=asc`

## Error Handling And Logs

- API uses global exception middleware and unified response model `LogDetails`.
- Validation failures also return `LogDetails` with `ValidationErrors`.
- Local log file:
  - `src/Lemax.HotelSearch.Api/logs/application-log.txt`
