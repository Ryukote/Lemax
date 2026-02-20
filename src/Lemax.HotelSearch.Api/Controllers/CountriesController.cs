using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Lemax.HotelSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Represents countries controller type.
/// </summary>
public class CountriesController : ControllerBase
{
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public CountriesController(ICountryService countryService, IMapper mapper)
    {
        _countryService = countryService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CountryDto>), StatusCodes.Status200OK)]
    /// <summary>
    /// Executes get all.
    /// </summary>
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var countries = await _countryService.GetAllAsync(cancellationToken);
        return Ok(countries);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CountryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    /// <summary>
    /// Executes get by id.
    /// </summary>
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var country = await _countryService.GetByIdAsync(id, cancellationToken);
        return country is null ? NotFound() : Ok(country);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CountryDto), StatusCodes.Status201Created)]
    /// <summary>
    /// Executes create.
    /// </summary>
    public async Task<IActionResult> Create([FromBody] CreateCountryHttpRequest request, CancellationToken cancellationToken)
    {
        var serviceRequest = _mapper.Map<CreateCountryRequest>(request);
        var created = await _countryService.CreateAsync(serviceRequest, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
