using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Lemax.HotelSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Represents hotels controller type.
/// </summary>
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IMapper _mapper;

    public HotelsController(IHotelService hotelService, IMapper mapper)
    {
        _hotelService = hotelService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<HotelDto>), StatusCodes.Status200OK)]
    /// <summary>
    /// Executes get all.
    /// </summary>
    public async Task<IActionResult> GetAll([FromQuery] GetHotelsQuery query, CancellationToken cancellationToken)
    {
        var serviceRequest = _mapper.Map<GetHotelsRequest>(query);
        var hotels = await _hotelService.GetPagedAsync(serviceRequest, cancellationToken);
        return Ok(hotels);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    /// <summary>
    /// Executes get by id.
    /// </summary>
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var hotel = await _hotelService.GetByIdAsync(id, cancellationToken);
        return hotel is null ? NotFound() : Ok(hotel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status201Created)]
    /// <summary>
    /// Executes create.
    /// </summary>
    public async Task<IActionResult> Create([FromBody] CreateHotelHttpRequest request, CancellationToken cancellationToken)
    {
        var serviceRequest = _mapper.Map<CreateHotelRequest>(request);
        var created = await _hotelService.CreateAsync(serviceRequest, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    /// <summary>
    /// Executes update.
    /// </summary>
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHotelHttpRequest request, CancellationToken cancellationToken)
    {
        var serviceRequest = _mapper.Map<UpdateHotelRequest>(request);
        var updated = await _hotelService.UpdateAsync(id, serviceRequest, cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    /// <summary>
    /// Executes delete.
    /// </summary>
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _hotelService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("search/all")]
    [ProducesResponseType(typeof(IReadOnlyList<SearchHotelResultDto>), StatusCodes.Status200OK)]
    /// <summary>
    /// Executes search all.
    /// </summary>
    public async Task<IActionResult> SearchAll([FromQuery] SearchHotelsAllQuery query, CancellationToken cancellationToken)
    {
        var serviceRequest = _mapper.Map<SearchHotelsAllRequest>(query);
        var result = await _hotelService.SearchAllAsync(serviceRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<SearchHotelResultDto>), StatusCodes.Status200OK)]
    /// <summary>
    /// Executes search.
    /// </summary>
    public async Task<IActionResult> Search([FromQuery] SearchHotelsQuery query, CancellationToken cancellationToken)
    {
        var serviceRequest = _mapper.Map<SearchHotelsRequest>(query);
        var result = await _hotelService.SearchAsync(serviceRequest, cancellationToken);

        return Ok(result);
    }
}
