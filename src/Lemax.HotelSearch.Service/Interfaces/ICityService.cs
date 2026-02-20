using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Service.Interfaces;

/// <summary>
/// Application contract for city use-cases.
/// </summary>
public interface ICityService
{
    /// <summary>
    /// Creates a city and returns the created DTO.
    /// </summary>
    Task<CityDto> CreateAsync(CreateCityRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a city by identifier, or null when not found.
    /// </summary>
    Task<CityDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all cities or only cities matching a country filter.
    /// </summary>
    Task<IReadOnlyList<CityDto>> GetAllAsync(Guid? countryId, CancellationToken cancellationToken = default);
}
