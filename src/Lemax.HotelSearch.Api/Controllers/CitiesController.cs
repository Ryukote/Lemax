using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Lemax.HotelSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// API endpoints for city management.
/// </summary>
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes controller dependencies.
    /// </summary>
    public CitiesController(ICityService cityService, IMapper mapper)
    {
        _cityService = cityService;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns all cities or cities filtered by country.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CityDto>), StatusCodes.Status200OK)]
    /// <summary>
    /// Executes get all.
    /// </summary>
    public async Task<IActionResult> GetAll([FromQuery] GetCitiesQuery query, CancellationToken cancellationToken)
    {
        var cities = await _cityService.GetAllAsync(query.CountryId, cancellationToken);
        return Ok(cities);
    }

    /// <summary>
    /// Returns a single city by identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    /// <summary>
    /// Executes get by id.
    /// </summary>
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var city = await _cityService.GetByIdAsync(id, cancellationToken);
        return city is null ? NotFound() : Ok(city);
    }

    /// <summary>
    /// Creates a new city.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CityDto), StatusCodes.Status201Created)]
    /// <summary>
    /// Executes create.
    /// </summary>
    public async Task<IActionResult> Create([FromBody] CreateCityHttpRequest request, CancellationToken cancellationToken)
    {
        var serviceRequest = _mapper.Map<CreateCityRequest>(request);
        var created = await _cityService.CreateAsync(serviceRequest, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
