using Lemax.HotelSearch.Infrastructure.Persistence;
using Lemax.HotelSearch.Service.Services;

namespace Lemax.HotelSearch.Test.Services;

public class CountryServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateCountry()
    {
        var service = new CountryService(new InMemoryCountryRepository(seed: false));

        var created = await service.CreateAsync(new() { Name = "Portugal", IsoCode = "PT" });

        Assert.Equal("Portugal", created.Name);
        Assert.Equal("PT", created.IsoCode);
    }
}
