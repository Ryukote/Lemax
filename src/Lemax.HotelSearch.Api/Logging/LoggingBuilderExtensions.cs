using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lemax.HotelSearch.Api.Logging;

/// <summary>
/// Represents logging builder extensions type.
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Executes add local file logger.
    /// </summary>
    public static ILoggingBuilder AddLocalFileLogger(this ILoggingBuilder builder, string filePath)
    {
        builder.Services.AddSingleton<ILoggerProvider>(_ => new FileLoggerProvider(filePath));
        return builder;
    }
}
