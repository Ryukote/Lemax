# Lemax Hotel Search Skeleton

[![CI](https://github.com/Ryukote/Lemax/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Ryukote/Lemax/actions/workflows/dotnet.yml)

Solution with project separation:

- `src/Lemax.HotelSearch.Api` - ASP.NET Core Web API (controllers, Swagger, validation)
- `src/Lemax.HotelSearch.Domain` - domain entities/value objects
- `src/Lemax.HotelSearch.Service` - application/service layer and search logic
- `src/Lemax.HotelSearch.Infrastructure` - in-memory repositories with preseed country/city data
- `tests/Lemax.HotelSearch.Test` - xUnit tests (unit + integration)

## Main API

- `GET /health`
- `GET /api/countries`
- `POST /api/countries`
- `GET /api/cities?countryId=...`
- `POST /api/cities`
- `GET /api/hotels` (paged + country/city filters)
- `POST /api/hotels`
- `PUT /api/hotels/{id}`
- `DELETE /api/hotels/{id}`
- `GET /api/hotels/search/all` (non-paged, all matching)
- `GET /api/hotels/search` (paged)

## Error Handling And Logging

- Global exception handling middleware returns unified `LogDetails` JSON responses.
- Validation errors also return `LogDetails` with `ValidationErrors` payload.
- Local text logs are written to:
  - `logs/application-log.txt`

## Notes

- `dotnet` SDK is required to build/run (`net8.0`).
- Search uses Haversine distance and default relevance score:
  - `relevance = 0.6 * distanceNorm + 0.4 * priceNorm`
- Hotel create/update validates that country exists, city exists, and city belongs to country.
- Infrastructure preseed exists only for countries and cities.
- Infrastructure does not preseed hotels outside tests.
