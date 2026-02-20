using System.Collections.Concurrent;
using Lemax.HotelSearch.Domain.Entities;
using Lemax.HotelSearch.Infrastructure.Seed;
using Lemax.HotelSearch.Service.Interfaces;

namespace Lemax.HotelSearch.Infrastructure.Persistence;

/// <summary>
/// Represents in memory country repository type.
/// </summary>
public class InMemoryCountryRepository : ICountryRepository
{
    private readonly ConcurrentDictionary<Guid, Country> _storage;

    public InMemoryCountryRepository(bool seed = true)
    {
        var initial = seed ? PreseedData.Countries : Array.Empty<Country>();
        _storage = new ConcurrentDictionary<Guid, Country>(initial.ToDictionary(x => x.Id, x => x));
    }

    /// <summary>
    /// Executes add async.
    /// </summary>
    public Task AddAsync(Country country, CancellationToken cancellationToken = default)
    {
        if (!_storage.TryAdd(country.Id, country))
        {
            throw new InvalidOperationException($"Country with id '{country.Id}' already exists.");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes get by id async.
    /// </summary>
    public Task<Country?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryGetValue(id, out var country);
        return Task.FromResult(country);
    }

    /// <summary>
    /// Executes get all async.
    /// </summary>
    public Task<IReadOnlyList<Country>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Country> countries = _storage.Values.ToArray();
        return Task.FromResult(countries);
    }
}
