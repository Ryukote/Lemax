using Lemax.HotelSearch.Domain.Entities;
using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Service.Interfaces;

/// <summary>
/// Persistence contract for hotel entities and hotel read projections.
/// </summary>
public interface IHotelRepository
{
    /// <summary>
    /// Persists a new hotel.
    /// </summary>
    Task AddAsync(Hotel hotel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the domain entity by identifier, or null when not found.
    /// </summary>
    Task<Hotel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a read model by identifier with city and country names.
    /// </summary>
    Task<HotelReadModel?> GetByIdWithLocationAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a paged read model result with city and country names.
    /// </summary>
    Task<PagedResult<HotelReadModel>> GetPagedWithLocationAsync(GetHotelsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns filtered read models used by search operations.
    /// </summary>
    Task<IReadOnlyList<HotelReadModel>> GetFilteredWithLocationAsync(SearchHotelsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing hotel.
    /// </summary>
    Task UpdateAsync(Hotel hotel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a hotel by identifier.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
