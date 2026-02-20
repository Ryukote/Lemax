using Lemax.HotelSearch.Infrastructure.Persistence;
using Lemax.HotelSearch.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Lemax.HotelSearch.Infrastructure.DependencyInjection;

/// <summary>
/// Represents service collection extensions type.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Executes add infrastructure.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICountryRepository, InMemoryCountryRepository>();
        services.AddSingleton<ICityRepository, InMemoryCityRepository>();
        services.AddSingleton<IHotelRepository, InMemoryHotelRepository>();
        return services;
    }
}
