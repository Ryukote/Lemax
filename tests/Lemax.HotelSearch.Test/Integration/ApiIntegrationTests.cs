using System.Net;
using System.Net.Http.Json;
using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Infrastructure.Seed;
using Lemax.HotelSearch.Service.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Lemax.HotelSearch.Test.Integration;

public class ApiIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiIntegrationTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task Health_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCountries_ShouldReturnPreseededCountries()
    {
        var response = await _client.GetAsync("/api/countries");

        response.EnsureSuccessStatusCode();
        var countries = await response.Content.ReadFromJsonAsync<List<CountryDto>>();

        Assert.NotNull(countries);
        Assert.NotEmpty(countries);
        Assert.Contains(countries, c => c.Name == "Croatia" && c.IsoCode == "HR");
    }

    [Fact]
    public async Task SearchAll_ShouldReturnNonPagedResults()
    {
        var createHotelRequest = new
        {
            name = "Integration Hotel",
            price = 123,
            cityId = PreseedData.Ids.Zagreb,
            latitude = 45.815,
            longitude = 15.981
        };

        var createResponse = await _client.PostAsJsonAsync("/api/hotels", createHotelRequest);
        createResponse.EnsureSuccessStatusCode();

        var response = await _client.GetAsync("/api/hotels/search/all?searchLatitude=45.815&searchLongitude=15.981&sortBy=relevance&order=asc");

        response.EnsureSuccessStatusCode();
        var results = await response.Content.ReadFromJsonAsync<List<SearchHotelResultDto>>();

        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.Contains(results, x => x.Name == "Integration Hotel");
    }

    [Fact]
    public async Task CreateHotel_ShouldReturnLogDetails_WhenCityDoesNotExist()
    {
        var request = new
        {
            name = "Invalid Mapping Hotel",
            price = 150,
            cityId = Guid.NewGuid(),
            latitude = 45.815,
            longitude = 15.981
        };

        var response = await _client.PostAsJsonAsync("/api/hotels", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var logDetails = await response.Content.ReadFromJsonAsync<LogDetails>();
        Assert.NotNull(logDetails);
        Assert.Equal(400, logDetails.StatusCode);
        Assert.Equal("Validation failed", logDetails.Title);
        Assert.NotNull(logDetails.ValidationErrors);
        Assert.True(logDetails.ValidationErrors!.SelectMany(x => x.Value).Any(x => x.Contains("City does not exist", StringComparison.OrdinalIgnoreCase)));
    }
}
