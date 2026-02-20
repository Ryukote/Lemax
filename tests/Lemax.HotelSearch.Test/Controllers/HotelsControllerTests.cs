using Lemax.HotelSearch.Api.Controllers;
using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;
using Lemax.HotelSearch.Test.Seed;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lemax.HotelSearch.Test.Controllers;

public class HotelsControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOk_WithPagedHotels()
    {
        var countryId = Guid.NewGuid();
        var cityId = Guid.NewGuid();

        var query = ControllerTestDataSeeder.BuildGetHotelsQuery(countryId, cityId);
        var serviceRequest = new GetHotelsRequest { Page = query.Page, PageSize = query.PageSize, CountryId = query.CountryId, CityId = query.CityId };
        var serviceResponse = ControllerTestDataSeeder.BuildPagedHotels(countryId, cityId);

        var serviceMock = new Mock<IHotelService>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<GetHotelsRequest>(query)).Returns(serviceRequest);
        serviceMock.Setup(x => x.GetPagedAsync(serviceRequest, It.IsAny<CancellationToken>())).ReturnsAsync(serviceResponse);

        var controller = new HotelsController(serviceMock.Object, mapperMock.Object);

        var result = await controller.GetAll(query, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<PagedResult<HotelDto>>(ok.Value);
        Assert.Equal(2, payload.TotalCount);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WithCreatedHotel()
    {
        var countryId = Guid.NewGuid();
        var cityId = Guid.NewGuid();
        var httpRequest = ControllerTestDataSeeder.BuildCreateHotelHttpRequest(cityId);
        var serviceRequest = new CreateHotelRequest
        {
            Name = httpRequest.Name,
            Price = httpRequest.Price,
            CityId = httpRequest.CityId,
            Latitude = httpRequest.Latitude,
            Longitude = httpRequest.Longitude
        };

        var created = new HotelDto
        {
            Id = Guid.NewGuid(),
            Name = httpRequest.Name,
            Price = httpRequest.Price,
            CountryId = countryId,
            CityId = cityId,
            Latitude = httpRequest.Latitude,
            Longitude = httpRequest.Longitude
        };

        var serviceMock = new Mock<IHotelService>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<CreateHotelRequest>(httpRequest)).Returns(serviceRequest);
        serviceMock.Setup(x => x.CreateAsync(serviceRequest, It.IsAny<CancellationToken>())).ReturnsAsync(created);

        var controller = new HotelsController(serviceMock.Object, mapperMock.Object);

        var result = await controller.Create(httpRequest, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var payload = Assert.IsType<HotelDto>(createdResult.Value);
        Assert.Equal(created.Id, payload.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenServiceUpdatesSuccessfully()
    {
        var cityId = Guid.NewGuid();
        var hotelId = Guid.NewGuid();

        var httpRequest = ControllerTestDataSeeder.BuildUpdateHotelHttpRequest(cityId);
        var serviceRequest = new UpdateHotelRequest
        {
            Name = httpRequest.Name,
            Price = httpRequest.Price,
            CityId = httpRequest.CityId,
            Latitude = httpRequest.Latitude,
            Longitude = httpRequest.Longitude
        };

        var serviceMock = new Mock<IHotelService>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<UpdateHotelRequest>(httpRequest)).Returns(serviceRequest);
        serviceMock.Setup(x => x.UpdateAsync(hotelId, serviceRequest, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var controller = new HotelsController(serviceMock.Object, mapperMock.Object);

        var result = await controller.Update(hotelId, httpRequest, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }
}
