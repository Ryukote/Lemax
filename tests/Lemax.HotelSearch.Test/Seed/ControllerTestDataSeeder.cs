using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Test.Seed;

public static class ControllerTestDataSeeder
{
    public static CreateHotelHttpRequest BuildCreateHotelHttpRequest(Guid cityId)
        => new()
        {
            Name = "Seed Created Hotel",
            Price = 110,
            CityId = cityId,
            Latitude = 45.812,
            Longitude = 15.978
        };

    public static UpdateHotelHttpRequest BuildUpdateHotelHttpRequest(Guid cityId)
        => new()
        {
            Name = "Seed Updated Hotel",
            Price = 140,
            CityId = cityId,
            Latitude = 45.813,
            Longitude = 15.979
        };

    public static GetHotelsQuery BuildGetHotelsQuery(Guid countryId, Guid cityId)
        => new()
        {
            Page = 1,
            PageSize = 2,
            CountryId = countryId,
            CityId = cityId
        };

    public static SearchHotelsQuery BuildSearchHotelsQuery(Guid countryId, Guid cityId)
        => new()
        {
            SearchLatitude = 45.815,
            SearchLongitude = 15.981,
            RadiusKm = 30,
            SortBy = "relevance",
            Order = "asc",
            Page = 1,
            PageSize = 5,
            CountryId = countryId,
            CityId = cityId
        };

    public static SearchHotelsAllQuery BuildSearchHotelsAllQuery(Guid countryId, Guid cityId)
        => new()
        {
            SearchLatitude = 45.815,
            SearchLongitude = 15.981,
            SortBy = "relevance",
            Order = "asc",
            CountryId = countryId,
            CityId = cityId
        };

    public static CreateCountryHttpRequest BuildCreateCountryHttpRequest()
        => new() { Name = "Portugal", IsoCode = "PT" };

    public static CreateCityHttpRequest BuildCreateCityHttpRequest(Guid countryId)
        => new()
        {
            Name = "Lisbon",
            CountryId = countryId,
            CenterLatitude = 38.7223,
            CenterLongitude = -9.1393
        };

    public static PagedResult<HotelDto> BuildPagedHotels(Guid countryId, Guid cityId)
        => new()
        {
            Items =
            [
                new HotelDto { Id = Guid.NewGuid(), Name = "Seed Hotel 1", Price = 100, CountryId = countryId, CityId = cityId, Latitude = 45.815, Longitude = 15.981 },
                new HotelDto { Id = Guid.NewGuid(), Name = "Seed Hotel 2", Price = 130, CountryId = countryId, CityId = cityId, Latitude = 45.820, Longitude = 15.987 }
            ],
            Page = 1,
            PageSize = 2,
            TotalCount = 2
        };

    public static PagedResult<SearchHotelResultDto> BuildSearchResults()
        => new()
        {
            Items =
            [
                new SearchHotelResultDto { Id = Guid.NewGuid(), Name = "Seed Search 1", Price = 95, DistanceKm = 1.2, RelevanceScore = 0.11 },
                new SearchHotelResultDto { Id = Guid.NewGuid(), Name = "Seed Search 2", Price = 115, DistanceKm = 2.8, RelevanceScore = 0.27 }
            ],
            Page = 1,
            PageSize = 5,
            TotalCount = 2
        };

    public static IReadOnlyList<SearchHotelResultDto> BuildSearchAllResults()
        =>
        [
            new SearchHotelResultDto { Id = Guid.NewGuid(), Name = "Seed Search All 1", Price = 90, DistanceKm = 0.9, RelevanceScore = 0.08 },
            new SearchHotelResultDto { Id = Guid.NewGuid(), Name = "Seed Search All 2", Price = 140, DistanceKm = 4.1, RelevanceScore = 0.36 }
        ];

    public static IReadOnlyList<CountryDto> BuildCountries()
        =>
        [
            new CountryDto { Id = Guid.NewGuid(), Name = "Croatia", IsoCode = "HR" },
            new CountryDto { Id = Guid.NewGuid(), Name = "Italy", IsoCode = "IT" }
        ];

    public static IReadOnlyList<CityDto> BuildCities(Guid countryId)
        =>
        [
            new CityDto { Id = Guid.NewGuid(), Name = "Zagreb", CountryId = countryId, CenterLatitude = 45.815, CenterLongitude = 15.981 },
            new CityDto { Id = Guid.NewGuid(), Name = "Split", CountryId = countryId, CenterLatitude = 43.5081, CenterLongitude = 16.4402 }
        ];
}
