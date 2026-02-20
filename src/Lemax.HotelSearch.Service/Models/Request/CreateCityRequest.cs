namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// Service request for creating a city.
/// </summary>
public class CreateCityRequest
{
    /// <summary>
    /// City display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Country identifier.
    /// </summary>
    public Guid CountryId { get; set; }

    /// <summary>
    /// Latitude of city center.
    /// </summary>
    public double CenterLatitude { get; set; }

    /// <summary>
    /// Longitude of city center.
    /// </summary>
    public double CenterLongitude { get; set; }
}
