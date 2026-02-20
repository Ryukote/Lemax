using Lemax.HotelSearch.Domain.Entities;
using Lemax.HotelSearch.Domain.ValueObjects;

namespace Lemax.HotelSearch.Infrastructure.Seed;

/// <summary>
/// Represents preseed data type.
/// </summary>
public static class PreseedData
{
    /// <summary>
    /// Represents ids type.
    /// </summary>
    public static class Ids
    {
        public static readonly Guid Croatia = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid Italy = Guid.Parse("22222222-2222-2222-2222-222222222222");

        public static readonly Guid Zagreb = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
        public static readonly Guid Split = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");
        public static readonly Guid Rome = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1");
    }

    public static IReadOnlyList<Country> Countries =>
    [
        new Country(Ids.Croatia, "Croatia", "HR"),
        new Country(Ids.Italy, "Italy", "IT")
    ];

    public static IReadOnlyList<City> Cities =>
    [
        new City(Ids.Zagreb, "Zagreb", Ids.Croatia, GeoPoint.Create(45.8150, 15.9819)),
        new City(Ids.Split, "Split", Ids.Croatia, GeoPoint.Create(43.5081, 16.4402)),
        new City(Ids.Rome, "Rome", Ids.Italy, GeoPoint.Create(41.9028, 12.4964))
    ];
}
