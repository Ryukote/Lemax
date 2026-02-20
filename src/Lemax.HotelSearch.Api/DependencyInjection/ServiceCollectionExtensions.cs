using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Models;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Lemax.HotelSearch.Api.DependencyInjection;

/// <summary>
/// Represents service collection extensions type.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Executes add api mappings.
    /// </summary>
    public static IServiceCollection AddApiMappings(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<GetHotelsQuery, GetHotelsRequest>();
        config.NewConfig<CreateHotelHttpRequest, CreateHotelRequest>();
        config.NewConfig<UpdateHotelHttpRequest, UpdateHotelRequest>();
        config.NewConfig<SearchHotelsQuery, SearchHotelsRequest>();
        config.NewConfig<SearchHotelsAllQuery, SearchHotelsAllRequest>();
        config.NewConfig<CreateCountryHttpRequest, CreateCountryRequest>();
        config.NewConfig<CreateCityHttpRequest, CreateCityRequest>();

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
