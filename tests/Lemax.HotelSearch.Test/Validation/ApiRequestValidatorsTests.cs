using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Api.Validation.Request;
using Lemax.HotelSearch.Infrastructure.Persistence;

namespace Lemax.HotelSearch.Test.Validation;

public class ApiRequestValidatorsTests
{
    [Fact]
    public void CreateCountryValidator_ShouldFail_WhenNameEmpty()
    {
        var validator = new CreateCountryHttpRequestValidator();
        var result = validator.Validate(new CreateCountryHttpRequest { Name = "", IsoCode = "HR" });
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task CreateHotelValidator_ShouldFail_WhenCityMissing()
    {
        var cityRepo = new InMemoryCityRepository(seed: false);
        var validator = new CreateHotelHttpRequestValidator(cityRepo);

        var result = await validator.ValidateAsync(new CreateHotelHttpRequest
        {
            Name = "Test",
            Price = 100,
            CityId = Guid.NewGuid(),
            Latitude = 45.8,
            Longitude = 15.9
        });

        Assert.False(result.IsValid);
    }
}
