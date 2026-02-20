namespace Lemax.HotelSearch.Api.Models;

/// <summary>
/// HTTP payload for creating a country.
/// </summary>
public class CreateCountryHttpRequest
{
    /// <summary>
    /// Country display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// ISO country code (for example, HR or IT).
    /// </summary>
    public string IsoCode { get; set; } = string.Empty;
}
