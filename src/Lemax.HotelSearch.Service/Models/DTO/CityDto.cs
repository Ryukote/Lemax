namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// City data returned by application services.
/// </summary>
public class CityDto
{
    /// <summary>
    /// City identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// City display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Related country identifier.
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
