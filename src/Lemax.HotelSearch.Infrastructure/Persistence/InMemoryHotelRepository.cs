using System.Collections.Concurrent;
using Lemax.HotelSearch.Domain.Entities;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Infrastructure.Persistence;

/// <summary>
/// Represents in memory hotel repository type.
/// </summary>
public class InMemoryHotelRepository : IHotelRepository
{
    private readonly ConcurrentDictionary<Guid, Hotel> _storage;
    private readonly ICityRepository? _cityRepository;
    private readonly ICountryRepository? _countryRepository;

    public InMemoryHotelRepository(
        ICityRepository? cityRepository = null,
        ICountryRepository? countryRepository = null,
        bool seed = true)
    {
        _storage = new ConcurrentDictionary<Guid, Hotel>();
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
    }

    /// <summary>
    /// Executes add async.
    /// </summary>
    public Task AddAsync(Hotel hotel, CancellationToken cancellationToken = default)
    {
        if (!_storage.TryAdd(hotel.Id, hotel))
        {
            throw new InvalidOperationException($"Hotel with id '{hotel.Id}' already exists.");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes get by id async.
    /// </summary>
    public Task<Hotel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryGetValue(id, out var hotel);
        return Task.FromResult(hotel);
    }

    /// <summary>
    /// Executes get paged with location async.
    /// </summary>
    public async Task<PagedResult<HotelReadModel>> GetPagedWithLocationAsync(GetHotelsRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Page <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Page), "Page must be greater than 0.");
        }

        if (request.PageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.PageSize), "PageSize must be greater than 0.");
        }

        var query = _storage.Values
            .Where(h => !request.CountryId.HasValue || h.CountryId == request.CountryId.Value)
            .Where(h => !request.CityId.HasValue || h.CityId == request.CityId.Value)
            .OrderBy(h => h.Name)
            .ThenBy(h => h.Id);

        var totalCount = query.Count();
        var pageItems = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArray();
        var items = await MapToReadModelsAsync(pageItems, cancellationToken);

        return new PagedResult<HotelReadModel>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Executes get filtered with location async.
    /// </summary>
    public async Task<IReadOnlyList<HotelReadModel>> GetFilteredWithLocationAsync(SearchHotelsRequest request, CancellationToken cancellationToken = default)
    {
        var hotels = _storage.Values
            .Where(h => !request.CountryId.HasValue || h.CountryId == request.CountryId.Value)
            .Where(h => !request.CityId.HasValue || h.CityId == request.CityId.Value)
            .Where(h => !request.MinPrice.HasValue || h.Price >= request.MinPrice.Value)
            .Where(h => !request.MaxPrice.HasValue || h.Price <= request.MaxPrice.Value)
            .ToArray();

        return await MapToReadModelsAsync(hotels, cancellationToken);
    }

    /// <summary>
    /// Executes get by id with location async.
    /// </summary>
    public async Task<HotelReadModel?> GetByIdWithLocationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var hotel = await GetByIdAsync(id, cancellationToken);
        if (hotel is null)
        {
            return null;
        }

        var mapped = await MapToReadModelsAsync([hotel], cancellationToken);
        return mapped.FirstOrDefault();
    }

    /// <summary>
    /// Executes update async.
    /// </summary>
    public Task UpdateAsync(Hotel hotel, CancellationToken cancellationToken = default)
    {
        _storage[hotel.Id] = hotel;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes delete async.
    /// </summary>
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    private async Task<IReadOnlyList<HotelReadModel>> MapToReadModelsAsync(IReadOnlyList<Hotel> hotels, CancellationToken cancellationToken)
    {
        IReadOnlyDictionary<Guid, string> cityNames = new Dictionary<Guid, string>();
        IReadOnlyDictionary<Guid, string> countryNames = new Dictionary<Guid, string>();

        if (_cityRepository is not null)
        {
            var cities = await _cityRepository.GetAllAsync(cancellationToken);
            cityNames = cities.ToDictionary(x => x.Id, x => x.Name);
        }

        if (_countryRepository is not null)
        {
            var countries = await _countryRepository.GetAllAsync(cancellationToken);
            countryNames = countries.ToDictionary(x => x.Id, x => x.Name);
        }

        return hotels.Select(h => new HotelReadModel
        {
            Id = h.Id,
            Name = h.Name,
            Price = h.Price,
            CountryId = h.CountryId,
            CountryName = countryNames.TryGetValue(h.CountryId, out var countryName) ? countryName : string.Empty,
            CityId = h.CityId,
            CityName = cityNames.TryGetValue(h.CityId, out var cityName) ? cityName : string.Empty,
            Latitude = h.Location.Latitude,
            Longitude = h.Location.Longitude
        }).ToArray();
    }
}
