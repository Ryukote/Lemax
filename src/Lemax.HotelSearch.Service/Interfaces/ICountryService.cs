using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Service.Interfaces;

/// <summary>
/// Application contract for country use-cases.
/// </summary>
public interface ICountryService
{
    /// <summary>
    /// Creates a country and returns the created DTO.
    /// </summary>
    Task<CountryDto> CreateAsync(CreateCountryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a country by identifier, or null when not found.
    /// </summary>
    Task<CountryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all countries.
    /// </summary>
    Task<IReadOnlyList<CountryDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
