namespace Lemax.HotelSearch.Service.Models;

/// <summary>
/// Represents paged result type.
/// </summary>
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    /// <summary>
    /// Gets or sets page.
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Gets or sets page size.
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// Gets or sets total count.
    /// </summary>
    public int TotalCount { get; set; }
}
