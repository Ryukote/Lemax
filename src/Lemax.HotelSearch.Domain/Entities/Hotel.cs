using Lemax.HotelSearch.Domain.ValueObjects;

namespace Lemax.HotelSearch.Domain.Entities;

/// <summary>
/// Domain entity representing a hotel.
/// </summary>
public class Hotel
{
    /// <summary>
    /// Unique hotel identifier.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Hotel display name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Nightly hotel price.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Identifier of the related country.
    /// </summary>
    public Guid CountryId { get; private set; }

    /// <summary>
    /// Identifier of the related city.
    /// </summary>
    public Guid CityId { get; private set; }

    /// <summary>
    /// Geographic location of the hotel.
    /// </summary>
    public GeoPoint Location { get; private set; }

    /// <summary>
    /// Initializes a hotel entity and validates invariants.
    /// </summary>
    public Hotel(Guid id, string name, decimal price, Guid countryId, Guid cityId, GeoPoint location)
    {
        if (id == Guid.Empty) throw new ArgumentException("Hotel id cannot be empty.", nameof(id));
        Validate(name, price, countryId, cityId);

        Id = id;
        Name = name.Trim();
        Price = price;
        CountryId = countryId;
        CityId = cityId;
        Location = location;
    }

    /// <summary>
    /// Updates mutable hotel fields after validating input.
    /// </summary>
    public void Update(string name, decimal price, Guid countryId, Guid cityId, GeoPoint location)
    {
        Validate(name, price, countryId, cityId);

        Name = name.Trim();
        Price = price;
        CountryId = countryId;
        CityId = cityId;
        Location = location;
    }

    /// <summary>
    /// Validates aggregate invariants for create and update flows.
    /// </summary>
    private static void Validate(string name, decimal price, Guid countryId, Guid cityId)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Hotel name is required.", nameof(name));
        if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price), "Hotel price must be greater than 0.");
        if (countryId == Guid.Empty) throw new ArgumentException("Country id cannot be empty.", nameof(countryId));
        if (cityId == Guid.Empty) throw new ArgumentException("City id cannot be empty.", nameof(cityId));
    }
}
