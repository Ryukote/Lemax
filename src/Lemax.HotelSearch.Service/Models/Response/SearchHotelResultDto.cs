namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// Represents search hotel result dto type.
/// </summary>
public class SearchHotelResultDto
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets price.
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Gets or sets country id.
    /// </summary>
    public Guid CountryId { get; set; }
    /// <summary>
    /// Gets or sets country name.
    /// </summary>
    public string CountryName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets city id.
    /// </summary>
    public Guid CityId { get; set; }
    /// <summary>
    /// Gets or sets city name.
    /// </summary>
    public string CityName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets distance km.
    /// </summary>
    public double DistanceKm { get; set; }
    /// <summary>
    /// Gets or sets relevance score.
    /// </summary>
    public double RelevanceScore { get; set; }
}
