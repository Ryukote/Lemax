using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Service.Interfaces;

/// <summary>
/// Application contract for hotel CRUD and search use-cases.
/// </summary>
public interface IHotelService
{
    /// <summary>
    /// Creates a hotel and returns the created DTO.
    /// </summary>
    Task<HotelDto> CreateAsync(CreateHotelRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a hotel by identifier, or null when not found.
    /// </summary>
    Task<HotelDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns paged hotels with optional filters.
    /// </summary>
    Task<PagedResult<HotelDto>> GetPagedAsync(GetHotelsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing hotel.
    /// </summary>
    Task<bool> UpdateAsync(Guid id, UpdateHotelRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing hotel.
    /// </summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all matching hotels without pagination.
    /// </summary>
    Task<IReadOnlyList<SearchHotelResultDto>> SearchAllAsync(SearchHotelsAllRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns paged matching hotels.
    /// </summary>
    Task<PagedResult<SearchHotelResultDto>> SearchAsync(SearchHotelsRequest request, CancellationToken cancellationToken = default);
}
