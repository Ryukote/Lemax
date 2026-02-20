namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// Service request for paged hotel search.
/// </summary>
public class SearchHotelsRequest
{
    /// <summary>
    /// Latitude of the reference point.
    /// </summary>
    public double SearchLatitude { get; set; }

    /// <summary>
    /// Longitude of the reference point.
    /// </summary>
    public double SearchLongitude { get; set; }

    /// <summary>
    /// Search radius in kilometers.
    /// </summary>
    public double RadiusKm { get; set; }

    /// <summary>
    /// Sort field (relevance, price, distance).
    /// </summary>
    public string SortBy { get; set; } = "relevance";

    /// <summary>
    /// Sort order (asc or desc).
    /// </summary>
    public string Order { get; set; } = "asc";

    /// <summary>
    /// Requested page number (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Optional minimum price filter.
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Optional maximum price filter.
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Optional country filter.
    /// </summary>
    public Guid? CountryId { get; set; }

    /// <summary>
    /// Optional city filter.
    /// </summary>
    public Guid? CityId { get; set; }
}
