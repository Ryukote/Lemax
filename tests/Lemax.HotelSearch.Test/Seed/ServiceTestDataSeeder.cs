using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Test.Seed;

public static class ServiceTestDataSeeder
{
    public static async Task<ServiceSeedContext> SeedHotelsAsync(
        ICountryService countryService,
        ICityService cityService,
        IHotelService hotelService)
    {
        var countryA = await countryService.CreateAsync(new CreateCountryRequest { Name = "Croatia", IsoCode = "HR" });
        var countryB = await countryService.CreateAsync(new CreateCountryRequest { Name = "Italy", IsoCode = "IT" });

        var cityA1 = await cityService.CreateAsync(new CreateCityRequest { Name = "Zagreb", CountryId = countryA.Id, CenterLatitude = 45.8150, CenterLongitude = 15.9819 });
        var cityA2 = await cityService.CreateAsync(new CreateCityRequest { Name = "Split", CountryId = countryA.Id, CenterLatitude = 43.5081, CenterLongitude = 16.4402 });
        var cityB1 = await cityService.CreateAsync(new CreateCityRequest { Name = "Rome", CountryId = countryB.Id, CenterLatitude = 41.9028, CenterLongitude = 12.4964 });

        var createdHotels = new List<HotelDto>
        {
            await hotelService.CreateAsync(new CreateHotelRequest { Name = "Alpha Center", Price = 90, CityId = cityA1.Id, Latitude = 45.8150, Longitude = 15.9819 }),
            await hotelService.CreateAsync(new CreateHotelRequest { Name = "Bravo Budget", Price = 70, CityId = cityA1.Id, Latitude = 45.8090, Longitude = 15.9700 }),
            await hotelService.CreateAsync(new CreateHotelRequest { Name = "Charlie Premium", Price = 180, CityId = cityA2.Id, Latitude = 45.8300, Longitude = 15.9900 }),
            await hotelService.CreateAsync(new CreateHotelRequest { Name = "Delta Coastal", Price = 120, CityId = cityB1.Id, Latitude = 41.9000, Longitude = 12.4900 }),
            await hotelService.CreateAsync(new CreateHotelRequest { Name = "Echo Hills", Price = 150, CityId = cityB1.Id, Latitude = 41.9100, Longitude = 12.5000 })
        };

        return new ServiceSeedContext
        {
            CountryAId = countryA.Id,
            CountryBId = countryB.Id,
            CityA1Id = cityA1.Id,
            CityA2Id = cityA2.Id,
            CityB1Id = cityB1.Id,
            Hotels = createdHotels
        };
    }
}

public class ServiceSeedContext
{
    public Guid CountryAId { get; set; }
    public Guid CountryBId { get; set; }
    public Guid CityA1Id { get; set; }
    public Guid CityA2Id { get; set; }
    public Guid CityB1Id { get; set; }
    public List<HotelDto> Hotels { get; set; } = new();
}
