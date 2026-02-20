namespace Lemax.HotelSearch.Api.Models;

/// <summary>
/// HTTP payload for creating a city.
/// </summary>
public class CreateCityHttpRequest
{
    /// <summary>
    /// City display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Country identifier that owns the city.
    /// </summary>
    public Guid CountryId { get; set; }

    /// <summary>
    /// Latitude of the city center point.
    /// </summary>
    public double CenterLatitude { get; set; }

    /// <summary>
    /// Longitude of the city center point.
    /// </summary>
    public double CenterLongitude { get; set; }
}
