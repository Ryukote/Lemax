namespace Lemax.HotelSearch.Api.Logging;

/// <summary>
/// Represents file logger type.
/// </summary>
public class FileLogger : ILogger
{
    private readonly string _categoryName;
    private readonly FileLoggerProvider _provider;

    public FileLogger(string categoryName, FileLoggerProvider provider)
    {
        _categoryName = categoryName;
        _provider = provider;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
        => NullScope.Instance;

    /// <summary>
    /// Executes is enabled.
    /// </summary>
    public bool IsEnabled(LogLevel logLevel)
        => logLevel >= LogLevel.Information;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        if (string.IsNullOrWhiteSpace(message) && exception is null)
        {
            return;
        }

        var line = $"{DateTimeOffset.UtcNow:O} [{logLevel}] {_categoryName}: {message}";
        if (exception is not null)
        {
            line += Environment.NewLine + exception;
        }

        _provider.WriteLine(line);
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        /// <summary>
        /// Executes dispose.
        /// </summary>
        public void Dispose() { }
    }
}
