namespace Lemax.HotelSearch.Api.Models;

/// <summary>
/// Standardized API error payload used by middleware and model validation.
/// </summary>
public class LogDetails
{
    /// <summary>
    /// HTTP status code returned to the client.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Short error title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Request path that produced the error.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// ASP.NET request trace identifier.
    /// </summary>
    public string TraceId { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the response was generated.
    /// </summary>
    public DateTimeOffset TimestampUtc { get; set; }

    /// <summary>
    /// Optional field-level validation messages.
    /// </summary>
    public IDictionary<string, string[]>? ValidationErrors { get; set; }
}
