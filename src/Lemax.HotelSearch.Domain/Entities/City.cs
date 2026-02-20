using Lemax.HotelSearch.Domain.ValueObjects;

namespace Lemax.HotelSearch.Domain.Entities;

/// <summary>
/// Domain entity representing a city.
/// </summary>
public class City
{
    /// <summary>
    /// Unique city identifier.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// City display name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Identifier of the related country.
    /// </summary>
    public Guid CountryId { get; private set; }

    /// <summary>
    /// Geographic center of the city.
    /// </summary>
    public GeoPoint Center { get; private set; }

    /// <summary>
    /// Initializes a city entity and validates its basic invariants.
    /// </summary>
    public City(Guid id, string name, Guid countryId, GeoPoint center)
    {
        if (id == Guid.Empty) throw new ArgumentException("City id cannot be empty.", nameof(id));
        if (countryId == Guid.Empty) throw new ArgumentException("Country id cannot be empty.", nameof(countryId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("City name is required.", nameof(name));

        Id = id;
        Name = name.Trim();
        CountryId = countryId;
        Center = center;
    }
}
