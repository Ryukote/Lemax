namespace Lemax.HotelSearch.Domain.Entities;

/// <summary>
/// Domain entity representing a country.
/// </summary>
public class Country
{
    /// <summary>
    /// Unique country identifier.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Country display name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// ISO country code.
    /// </summary>
    public string IsoCode { get; private set; }

    /// <summary>
    /// Initializes a country entity and validates its basic invariants.
    /// </summary>
    public Country(Guid id, string name, string isoCode)
    {
        if (id == Guid.Empty) throw new ArgumentException("Country id cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Country name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(isoCode)) throw new ArgumentException("Country ISO code is required.", nameof(isoCode));

        Id = id;
        Name = name.Trim();
        IsoCode = isoCode.Trim().ToUpperInvariant();
    }
}
