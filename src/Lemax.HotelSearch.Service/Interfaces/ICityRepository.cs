using Lemax.HotelSearch.Domain.Entities;

namespace Lemax.HotelSearch.Service.Interfaces;

/// <summary>
/// Persistence contract for city entities.
/// </summary>
public interface ICityRepository
{
    /// <summary>
    /// Persists a new city.
    /// </summary>
    Task AddAsync(City city, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a city by identifier, or null when not found.
    /// </summary>
    Task<City?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all stored cities.
    /// </summary>
    Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all cities for a given country.
    /// </summary>
    Task<IReadOnlyList<City>> GetByCountryIdAsync(Guid countryId, CancellationToken cancellationToken = default);
}
