using Lemax.HotelSearch.Domain.Entities;

namespace Lemax.HotelSearch.Service.Interfaces;

/// <summary>
/// Persistence contract for country entities.
/// </summary>
public interface ICountryRepository
{
    /// <summary>
    /// Persists a new country.
    /// </summary>
    Task AddAsync(Country country, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a country by identifier, or null when not found.
    /// </summary>
    Task<Country?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all stored countries.
    /// </summary>
    Task<IReadOnlyList<Country>> GetAllAsync(CancellationToken cancellationToken = default);
}
