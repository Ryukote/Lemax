namespace Lemax.HotelSearch.Api.Models;

/// <summary>
/// Query parameters for paged hotel listing.
/// </summary>
public class GetHotelsQuery
{
    /// <summary>
    /// Requested page number (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Optional country filter.
    /// </summary>
    public Guid? CountryId { get; set; }

    /// <summary>
    /// Optional city filter.
    /// </summary>
    public Guid? CityId { get; set; }
}
