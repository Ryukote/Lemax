using Lemax.HotelSearch.Domain.Entities;
using Lemax.HotelSearch.Domain.ValueObjects;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Service.Services;

/// <summary>
/// Represents hotel service type.
/// </summary>
public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly ICityRepository _cityRepository;

    public HotelService(
        IHotelRepository hotelRepository,
        ICityRepository cityRepository)
    {
        _hotelRepository = hotelRepository;
        _cityRepository = cityRepository;
    }

    /// <summary>
    /// Executes create async.
    /// </summary>
    public async Task<HotelDto> CreateAsync(CreateHotelRequest request, CancellationToken cancellationToken = default)
    {
        var city = await EnsureCityExistsAsync(request.CityId, cancellationToken);

        var hotel = new Hotel(
            Guid.NewGuid(),
            request.Name,
            request.Price,
            city.CountryId,
            request.CityId,
            GeoPoint.Create(request.Latitude, request.Longitude));

        await _hotelRepository.AddAsync(hotel, cancellationToken);
        var created = await _hotelRepository.GetByIdWithLocationAsync(hotel.Id, cancellationToken);
        if (created is null)
        {
            throw new InvalidOperationException($"Hotel with id '{hotel.Id}' was not found after creation.");
        }

        return Map(created);
    }

    /// <summary>
    /// Executes get by id async.
    /// </summary>
    public async Task<HotelDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdWithLocationAsync(id, cancellationToken);
        return hotel is null ? null : Map(hotel);
    }

    /// <summary>
    /// Executes get paged async.
    /// </summary>
    public async Task<PagedResult<HotelDto>> GetPagedAsync(GetHotelsRequest request, CancellationToken cancellationToken = default)
    {
        var pagedHotels = await _hotelRepository.GetPagedWithLocationAsync(request, cancellationToken);

        return new PagedResult<HotelDto>
        {
            Items = pagedHotels.Items.Select(Map).ToArray(),
            Page = pagedHotels.Page,
            PageSize = pagedHotels.PageSize,
            TotalCount = pagedHotels.TotalCount
        };
    }

    /// <summary>
    /// Executes update async.
    /// </summary>
    public async Task<bool> UpdateAsync(Guid id, UpdateHotelRequest request, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);
        if (hotel is null)
        {
            return false;
        }

        var city = await EnsureCityExistsAsync(request.CityId, cancellationToken);

        hotel.Update(
            request.Name,
            request.Price,
            city.CountryId,
            request.CityId,
            GeoPoint.Create(request.Latitude, request.Longitude));

        await _hotelRepository.UpdateAsync(hotel, cancellationToken);
        return true;
    }

    /// <summary>
    /// Executes delete async.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _hotelRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        await _hotelRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    /// <summary>
    /// Executes search all async.
    /// </summary>
    public async Task<IReadOnlyList<SearchHotelResultDto>> SearchAllAsync(SearchHotelsAllRequest request, CancellationToken cancellationToken = default)
    {
        var candidates = await _hotelRepository.GetFilteredWithLocationAsync(
            new SearchHotelsRequest
            {
                SearchLatitude = request.SearchLatitude,
                SearchLongitude = request.SearchLongitude,
                RadiusKm = double.MaxValue,
                SortBy = request.SortBy,
                Order = request.Order,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                CountryId = request.CountryId,
                CityId = request.CityId,
                Page = 1,
                PageSize = int.MaxValue
            },
            cancellationToken);

        var ordered = BuildOrderedSearchResults(
            candidates,
            request.SearchLatitude,
            request.SearchLongitude,
            null,
            request.SortBy,
            request.Order);

        return ordered.ToArray();
    }

    /// <summary>
    /// Executes search async.
    /// </summary>
    public async Task<PagedResult<SearchHotelResultDto>> SearchAsync(SearchHotelsRequest request, CancellationToken cancellationToken = default)
    {
        ValidateSearchInputs(request.RadiusKm, request.Page, request.PageSize);

        var candidateHotels = await _hotelRepository.GetFilteredWithLocationAsync(request, cancellationToken);

        var ordered = BuildOrderedSearchResults(
            candidateHotels,
            request.SearchLatitude,
            request.SearchLongitude,
            request.RadiusKm,
            request.SortBy,
            request.Order).ToArray();

        var totalCount = ordered.Length;

        var items = ordered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArray();

        return new PagedResult<SearchHotelResultDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    private async Task<City> EnsureCityExistsAsync(Guid cityId, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(cityId, cancellationToken);
        if (city is null)
        {
            throw new ArgumentException("City does not exist.", nameof(cityId));
        }

        return city;
    }

    private static IEnumerable<SearchHotelResultDto> BuildOrderedSearchResults(
        IReadOnlyList<HotelReadModel> candidateHotels,
        double searchLatitude,
        double searchLongitude,
        double? radiusKm,
        string sortBy,
        string order)
    {
        var bboxFilteredHotels = radiusKm.HasValue
            ? ApplyBoundingBoxFilter(candidateHotels, searchLatitude, searchLongitude, radiusKm.Value)
            : candidateHotels;

        var withDistance = bboxFilteredHotels
            .Select(h => new
            {
                Hotel = h,
                Distance = HaversineDistanceCalculator.CalculateKm(
                    searchLatitude,
                    searchLongitude,
                    h.Latitude,
                    h.Longitude)
            });

        var filtered = radiusKm.HasValue
            ? withDistance.Where(x => x.Distance <= radiusKm.Value).ToArray()
            : withDistance.ToArray();

        if (filtered.Length == 0)
        {
            return Array.Empty<SearchHotelResultDto>();
        }

        var minPrice = filtered.Min(x => x.Hotel.Price);
        var maxPrice = filtered.Max(x => x.Hotel.Price);
        var minDistance = filtered.Min(x => x.Distance);
        var maxDistance = filtered.Max(x => x.Distance);

        var scored = filtered.Select(x =>
        {
            var priceNorm = Normalize((double)x.Hotel.Price, (double)minPrice, (double)maxPrice);
            var distanceNorm = Normalize(x.Distance, minDistance, maxDistance);

            var relevanceScore = 0.6 * distanceNorm + 0.4 * priceNorm;

            return new SearchHotelResultDto
            {
                Id = x.Hotel.Id,
                Name = x.Hotel.Name,
                Price = x.Hotel.Price,
                CountryId = x.Hotel.CountryId,
                CountryName = x.Hotel.CountryName,
                CityId = x.Hotel.CityId,
                CityName = x.Hotel.CityName,
                DistanceKm = Math.Round(x.Distance, 2),
                RelevanceScore = Math.Round(relevanceScore, 6)
            };
        });

        return ApplySorting(scored, sortBy, order);
    }

    private static IReadOnlyList<HotelReadModel> ApplyBoundingBoxFilter(
        IReadOnlyList<HotelReadModel> hotels,
        double searchLatitude,
        double searchLongitude,
        double radiusKm)
    {
        var latDelta = radiusKm / 111.32;
        var minLat = Math.Max(-90, searchLatitude - latDelta);
        var maxLat = Math.Min(90, searchLatitude + latDelta);

        var lonKmPerDegree = 111.32 * Math.Cos(searchLatitude * Math.PI / 180.0);
        if (Math.Abs(lonKmPerDegree) < 1e-9)
        {
            lonKmPerDegree = 1e-9;
        }

        var lonDelta = radiusKm / Math.Abs(lonKmPerDegree);
        var minLon = Math.Max(-180, searchLongitude - lonDelta);
        var maxLon = Math.Min(180, searchLongitude + lonDelta);

        return hotels
            .Where(h => h.Latitude >= minLat && h.Latitude <= maxLat)
            .Where(h => h.Longitude >= minLon && h.Longitude <= maxLon)
            .ToArray();
    }

    private static void ValidateSearchInputs(double radiusKm, int? page = null, int? pageSize = null)
    {
        if (radiusKm <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(radiusKm), "Radius must be greater than 0.");
        }

        if (page.HasValue && page.Value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0.");
        }

        if (pageSize.HasValue && pageSize.Value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be greater than 0.");
        }
    }

    private static IEnumerable<SearchHotelResultDto> ApplySorting(IEnumerable<SearchHotelResultDto> hotels, string sortBy, string order)
    {
        var isDesc = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);

        var sorted = sortBy.ToLowerInvariant() switch
        {
            "price" => isDesc
                ? hotels.OrderByDescending(x => x.Price).ThenBy(x => x.DistanceKm).ThenBy(x => x.Id)
                : hotels.OrderBy(x => x.Price).ThenBy(x => x.DistanceKm).ThenBy(x => x.Id),
            "distance" => isDesc
                ? hotels.OrderByDescending(x => x.DistanceKm).ThenBy(x => x.Price).ThenBy(x => x.Id)
                : hotels.OrderBy(x => x.DistanceKm).ThenBy(x => x.Price).ThenBy(x => x.Id),
            _ => isDesc
                ? hotels.OrderByDescending(x => x.RelevanceScore).ThenBy(x => x.DistanceKm).ThenBy(x => x.Price).ThenBy(x => x.Id)
                : hotels.OrderBy(x => x.RelevanceScore).ThenBy(x => x.DistanceKm).ThenBy(x => x.Price).ThenBy(x => x.Id)
        };

        return sorted;
    }

    private static double Normalize(double value, double min, double max)
    {
        if (Math.Abs(max - min) < 1e-9)
        {
            return 0;
        }

        return (value - min) / (max - min);
    }

    private static HotelDto Map(HotelReadModel hotel)
        => new()
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Price = hotel.Price,
            CountryId = hotel.CountryId,
            CountryName = hotel.CountryName,
            CityId = hotel.CityId,
            CityName = hotel.CityName,
            Latitude = hotel.Latitude,
            Longitude = hotel.Longitude
        };
}
