namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// Service request for creating a hotel.
/// </summary>
public class CreateHotelRequest
{
    /// <summary>
    /// Hotel display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Nightly hotel price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// City identifier where the hotel is located.
    /// </summary>
    public Guid CityId { get; set; }

    /// <summary>
    /// Hotel latitude.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Hotel longitude.
    /// </summary>
    public double Longitude { get; set; }
}
