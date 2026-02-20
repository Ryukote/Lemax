using Lemax.HotelSearch.Api.DependencyInjection;
using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Models;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Lemax.HotelSearch.Test.Mapping;

public class ApiMapsterMappingTests
{
    [Fact]
    public void ShouldMap_CreateCityHttpRequest_To_CreateCityRequest()
    {
        var services = new ServiceCollection();
        services.AddApiMappings();
        var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        var source = new CreateCityHttpRequest
        {
            Name = "Zagreb",
            CountryId = Guid.NewGuid(),
            CenterLatitude = 45.815,
            CenterLongitude = 15.981
        };

        var mapped = mapper.Map<CreateCityRequest>(source);

        Assert.Equal(source.Name, mapped.Name);
        Assert.Equal(source.CountryId, mapped.CountryId);
    }
}
