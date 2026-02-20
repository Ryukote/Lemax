namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// Country data returned by application services.
/// </summary>
public class CountryDto
{
    /// <summary>
    /// Country identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Country display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// ISO country code.
    /// </summary>
    public string IsoCode { get; set; } = string.Empty;
}
