using Lemax.HotelSearch.Infrastructure.Persistence;
using Lemax.HotelSearch.Service.Services;

namespace Lemax.HotelSearch.Test.Services;

public class CityServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCountryMissing()
    {
        var countryRepo = new InMemoryCountryRepository(seed: false);
        var cityRepo = new InMemoryCityRepository(seed: false);
        var service = new CityService(cityRepo, countryRepo);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(new()
        {
            Name = "Lisbon",
            CountryId = Guid.NewGuid(),
            CenterLatitude = 38.7223,
            CenterLongitude = -9.1393
        }));
    }
}
