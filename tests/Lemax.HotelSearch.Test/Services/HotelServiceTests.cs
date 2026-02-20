using Lemax.HotelSearch.Infrastructure.Persistence;
using Lemax.HotelSearch.Service.Models;
using Lemax.HotelSearch.Service.Services;
using Lemax.HotelSearch.Test.Seed;

namespace Lemax.HotelSearch.Test.Services;

public class HotelServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldPersistAndReturnHotel()
    {
        var (countryService, cityService, hotelService) = BuildServices();

        var country = await countryService.CreateAsync(new() { Name = "Croatia", IsoCode = "HR" });
        var city = await cityService.CreateAsync(new()
        {
            Name = "Zagreb",
            CountryId = country.Id,
            CenterLatitude = 45.815,
            CenterLongitude = 15.981
        });

        var created = await hotelService.CreateAsync(new()
        {
            Name = "Created Hotel",
            Price = 99,
            CityId = city.Id,
            Latitude = 45.81,
            Longitude = 15.97
        });

        var loaded = await hotelService.GetByIdAsync(created.Id);

        Assert.NotNull(loaded);
        Assert.Equal(country.Id, loaded.CountryId);
        Assert.Equal(country.Name, loaded.CountryName);
        Assert.Equal(city.Id, loaded.CityId);
        Assert.Equal(city.Name, loaded.CityName);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCityMissing()
    {
        var (_, _, hotelService) = BuildServices();

        await Assert.ThrowsAsync<ArgumentException>(() => hotelService.CreateAsync(new()
        {
            Name = "Invalid Hotel",
            Price = 100,
            CityId = Guid.NewGuid(),
            Latitude = 45.81,
            Longitude = 15.97
        }));
    }

    [Fact]
    public async Task SearchAllAsync_ShouldUseProvidedUserLocation()
    {
        var (countryService, cityService, hotelService) = BuildServices();
        await ServiceTestDataSeeder.SeedHotelsAsync(countryService, cityService, hotelService);

        var results = await hotelService.SearchAllAsync(new SearchHotelsAllRequest
        {
            // Deliberately far away from seeded cities; distances should reflect this location.
            SearchLatitude = -33.8688,
            SearchLongitude = 151.2093,
            SortBy = "relevance",
            Order = "asc"
        });

        Assert.NotEmpty(results);
        Assert.All(results, x => Assert.True(x.DistanceKm > 1_000));
    }

    private static (CountryService CountryService, CityService CityService, HotelService HotelService) BuildServices()
    {
        var countryRepo = new InMemoryCountryRepository(seed: false);
        var cityRepo = new InMemoryCityRepository(seed: false);
        var hotelRepo = new InMemoryHotelRepository(cityRepo, countryRepo, seed: false);

        var countryService = new CountryService(countryRepo);
        var cityService = new CityService(cityRepo, countryRepo);
        var hotelService = new HotelService(hotelRepo, cityRepo);

        return (countryService, cityService, hotelService);
    }
}
