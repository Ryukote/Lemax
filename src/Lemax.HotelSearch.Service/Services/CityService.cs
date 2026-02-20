using Lemax.HotelSearch.Domain.Entities;
using Lemax.HotelSearch.Domain.ValueObjects;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Service.Services;

/// <summary>
/// Represents city service type.
/// </summary>
public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;

    public CityService(ICityRepository cityRepository, ICountryRepository countryRepository)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
    }

    /// <summary>
    /// Executes create async.
    /// </summary>
    public async Task<CityDto> CreateAsync(CreateCityRequest request, CancellationToken cancellationToken = default)
    {
        var country = await _countryRepository.GetByIdAsync(request.CountryId, cancellationToken);
        if (country is null)
        {
            throw new ArgumentException("Country does not exist.", nameof(request.CountryId));
        }

        var city = new City(
            Guid.NewGuid(),
            request.Name,
            request.CountryId,
            GeoPoint.Create(request.CenterLatitude, request.CenterLongitude));

        await _cityRepository.AddAsync(city, cancellationToken);
        return Map(city);
    }

    /// <summary>
    /// Executes get by id async.
    /// </summary>
    public async Task<CityDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var city = await _cityRepository.GetByIdAsync(id, cancellationToken);
        return city is null ? null : Map(city);
    }

    /// <summary>
    /// Executes get all async.
    /// </summary>
    public async Task<IReadOnlyList<CityDto>> GetAllAsync(Guid? countryId, CancellationToken cancellationToken = default)
    {
        var cities = countryId.HasValue
            ? await _cityRepository.GetByCountryIdAsync(countryId.Value, cancellationToken)
            : await _cityRepository.GetAllAsync(cancellationToken);

        return cities.Select(Map).OrderBy(x => x.Name).ToArray();
    }

    private static CityDto Map(City city)
        => new()
        {
            Id = city.Id,
            Name = city.Name,
            CountryId = city.CountryId,
            CenterLatitude = city.Center.Latitude,
            CenterLongitude = city.Center.Longitude
        };
}
