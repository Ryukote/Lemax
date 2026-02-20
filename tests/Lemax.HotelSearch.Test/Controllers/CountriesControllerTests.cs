using Lemax.HotelSearch.Api.Controllers;
using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Service.Interfaces;
using Lemax.HotelSearch.Service.Models;
using Lemax.HotelSearch.Test.Seed;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lemax.HotelSearch.Test.Controllers;

public class CountriesControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        var serviceMock = new Mock<ICountryService>();
        var mapperMock = new Mock<IMapper>();
        serviceMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(ControllerTestDataSeeder.BuildCountries());

        var controller = new CountriesController(serviceMock.Object, mapperMock.Object);
        var result = await controller.GetAll(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsAssignableFrom<IReadOnlyList<CountryDto>>(ok.Value);
        Assert.NotEmpty(payload);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        var request = ControllerTestDataSeeder.BuildCreateCountryHttpRequest();
        var serviceRequest = new CreateCountryRequest { Name = request.Name, IsoCode = request.IsoCode };
        var created = new CountryDto { Id = Guid.NewGuid(), Name = request.Name, IsoCode = request.IsoCode };

        var serviceMock = new Mock<ICountryService>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<CreateCountryRequest>(request)).Returns(serviceRequest);
        serviceMock.Setup(x => x.CreateAsync(serviceRequest, It.IsAny<CancellationToken>())).ReturnsAsync(created);

        var controller = new CountriesController(serviceMock.Object, mapperMock.Object);
        var result = await controller.Create(request, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var payload = Assert.IsType<CountryDto>(createdResult.Value);
        Assert.Equal(created.Id, payload.Id);
    }
}
