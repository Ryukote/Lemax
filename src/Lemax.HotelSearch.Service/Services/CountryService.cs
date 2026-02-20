using Lemax.HotelSearch.Domain.Entities;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;

namespace Lemax.HotelSearch.Service.Services;

/// <summary>
/// Represents country service type.
/// </summary>
public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;

    public CountryService(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    /// <summary>
    /// Executes create async.
    /// </summary>
    public async Task<CountryDto> CreateAsync(CreateCountryRequest request, CancellationToken cancellationToken = default)
    {
        var country = new Country(Guid.NewGuid(), request.Name, request.IsoCode);
        await _countryRepository.AddAsync(country, cancellationToken);
        return Map(country);
    }

    /// <summary>
    /// Executes get by id async.
    /// </summary>
    public async Task<CountryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var country = await _countryRepository.GetByIdAsync(id, cancellationToken);
        return country is null ? null : Map(country);
    }

    /// <summary>
    /// Executes get all async.
    /// </summary>
    public async Task<IReadOnlyList<CountryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var countries = await _countryRepository.GetAllAsync(cancellationToken);
        return countries.Select(Map).OrderBy(x => x.Name).ToArray();
    }

    private static CountryDto Map(Country country)
        => new()
        {
            Id = country.Id,
            Name = country.Name,
            IsoCode = country.IsoCode
        };
}
