namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// Repository read projection with resolved city and country names.
/// </summary>
public class HotelReadModel
{
    /// <summary>
    /// Hotel identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Hotel display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Nightly hotel price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Country identifier.
    /// </summary>
    public Guid CountryId { get; set; }

    /// <summary>
    /// Country display name.
    /// </summary>
    public string CountryName { get; set; } = string.Empty;

    /// <summary>
    /// City identifier.
    /// </summary>
    public Guid CityId { get; set; }

    /// <summary>
    /// City display name.
    /// </summary>
    public string CityName { get; set; } = string.Empty;

    /// <summary>
    /// Hotel latitude.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Hotel longitude.
    /// </summary>
    public double Longitude { get; set; }
}
