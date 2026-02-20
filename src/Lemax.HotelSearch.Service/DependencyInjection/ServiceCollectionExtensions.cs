using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lemax.HotelSearch.Service.DependencyInjection;

/// <summary>
/// Represents service collection extensions type.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Executes add application services.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<ICityService, CityService>();
        return services;
    }
}
