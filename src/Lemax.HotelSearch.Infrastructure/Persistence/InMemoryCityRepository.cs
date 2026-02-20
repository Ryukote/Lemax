using System.Collections.Concurrent;
using Lemax.HotelSearch.Domain.Entities;
using Lemax.HotelSearch.Infrastructure.Seed;
using Lemax.HotelSearch.Service.Interfaces;

namespace Lemax.HotelSearch.Infrastructure.Persistence;

/// <summary>
/// Represents in memory city repository type.
/// </summary>
public class InMemoryCityRepository : ICityRepository
{
    private readonly ConcurrentDictionary<Guid, City> _storage;

    public InMemoryCityRepository(bool seed = true)
    {
        var initial = seed ? PreseedData.Cities : Array.Empty<City>();
        _storage = new ConcurrentDictionary<Guid, City>(initial.ToDictionary(x => x.Id, x => x));
    }

    /// <summary>
    /// Executes add async.
    /// </summary>
    public Task AddAsync(City city, CancellationToken cancellationToken = default)
    {
        if (!_storage.TryAdd(city.Id, city))
        {
            throw new InvalidOperationException($"City with id '{city.Id}' already exists.");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes get by id async.
    /// </summary>
    public Task<City?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryGetValue(id, out var city);
        return Task.FromResult(city);
    }

    /// <summary>
    /// Executes get all async.
    /// </summary>
    public Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<City> cities = _storage.Values.ToArray();
        return Task.FromResult(cities);
    }

    /// <summary>
    /// Executes get by country id async.
    /// </summary>
    public Task<IReadOnlyList<City>> GetByCountryIdAsync(Guid countryId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<City> cities = _storage.Values.Where(x => x.CountryId == countryId).ToArray();
        return Task.FromResult(cities);
    }
}
