namespace Lemax.HotelSearch.Domain.ValueObjects;

/// <summary>
/// Immutable latitude/longitude value object.
/// </summary>
public readonly record struct GeoPoint(double Latitude, double Longitude)
{
    /// <summary>
    /// Factory method that validates geographic coordinate bounds.
    /// </summary>
    public static GeoPoint Create(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be in range [-90, 90].");
        }

        if (longitude is < -180 or > 180)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be in range [-180, 180].");
        }

        return new GeoPoint(latitude, longitude);
    }
}
