using System.Text.Json;
using FluentValidation;
using Lemax.HotelSearch.Api.Models;

namespace Lemax.HotelSearch.Api.Middleware;

/// <summary>
/// Represents global exception handling middleware type.
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Executes invoke async.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Validation failed", ex.Message, ex);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Invalid request", ex.Message, ex);
        }
        catch (ArgumentException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Invalid request", ex.Message, ex);
        }
        catch (Exception ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "Unexpected error", "An unexpected error occurred.", ex);
        }
    }

    private async Task WriteErrorAsync(HttpContext context, int statusCode, string title, string message, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception for {Method} {Path}.", context.Request.Method, context.Request.Path);

        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new LogDetails
        {
            StatusCode = statusCode,
            Title = title,
            Message = message,
            Path = context.Request.Path,
            TraceId = context.TraceIdentifier,
            TimestampUtc = DateTimeOffset.UtcNow
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
