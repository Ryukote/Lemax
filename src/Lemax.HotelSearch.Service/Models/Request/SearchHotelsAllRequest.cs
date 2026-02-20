namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// Service request for non-paged hotel search.
/// </summary>
public class SearchHotelsAllRequest
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
    /// Sort field (relevance, price, distance).
    /// </summary>
    public string SortBy { get; set; } = "relevance";

    /// <summary>
    /// Sort order (asc or desc).
    /// </summary>
    public string Order { get; set; } = "asc";

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
