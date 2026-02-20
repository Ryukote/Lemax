using System.Collections.Concurrent;

namespace Lemax.HotelSearch.Api.Logging;

/// <summary>
/// Represents file logger provider type.
/// </summary>
public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;
    private readonly ConcurrentDictionary<string, FileLogger> _loggers = new();
    private readonly object _sync = new();

    public FileLoggerProvider(string filePath)
    {
        _filePath = filePath;
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    /// <summary>
    /// Executes create logger.
    /// </summary>
    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName, name => new FileLogger(name, this));

    /// <summary>
    /// Executes dispose.
    /// </summary>
    public void Dispose()
    {
    }

    internal void WriteLine(string line)
    {
        lock (_sync)
        {
            File.AppendAllText(_filePath, line + Environment.NewLine);
        }
    }
}
