namespace Lemax.HotelSearch.Api.Models;

/// <summary>
/// Query parameters for listing cities.
/// </summary>
public class GetCitiesQuery
{
    /// <summary>
    /// Optional country filter.
    /// </summary>
    public Guid? CountryId { get; set; }
}
