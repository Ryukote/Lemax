using Lemax.HotelSearch.Api.Controllers;
using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;
using Lemax.HotelSearch.Test.Seed;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lemax.HotelSearch.Test.Controllers;

public class CitiesControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        var countryId = Guid.NewGuid();
        var serviceMock = new Mock<ICityService>();
        var mapperMock = new Mock<IMapper>();

        serviceMock.Setup(x => x.GetAllAsync(countryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ControllerTestDataSeeder.BuildCities(countryId));

        var controller = new CitiesController(serviceMock.Object, mapperMock.Object);
        var result = await controller.GetAll(new GetCitiesQuery { CountryId = countryId }, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsAssignableFrom<IReadOnlyList<CityDto>>(ok.Value);
        Assert.NotEmpty(payload);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        var countryId = Guid.NewGuid();
        var request = ControllerTestDataSeeder.BuildCreateCityHttpRequest(countryId);
        var serviceRequest = new CreateCityRequest
        {
            Name = request.Name,
            CountryId = request.CountryId,
            CenterLatitude = request.CenterLatitude,
            CenterLongitude = request.CenterLongitude
        };

        var created = new CityDto
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CountryId = request.CountryId,
            CenterLatitude = request.CenterLatitude,
            CenterLongitude = request.CenterLongitude
        };

        var serviceMock = new Mock<ICityService>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<CreateCityRequest>(request)).Returns(serviceRequest);
        serviceMock.Setup(x => x.CreateAsync(serviceRequest, It.IsAny<CancellationToken>())).ReturnsAsync(created);

        var controller = new CitiesController(serviceMock.Object, mapperMock.Object);
        var result = await controller.Create(request, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var payload = Assert.IsType<CityDto>(createdResult.Value);
        Assert.Equal(created.Id, payload.Id);
    }
}
