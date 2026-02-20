namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// Service request for creating a country.
/// </summary>
public class CreateCountryRequest
{
    /// <summary>
    /// Country display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// ISO country code.
    /// </summary>
    public string IsoCode { get; set; } = string.Empty;
}
