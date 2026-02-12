using System.Net;
using HomeEnergyApi.Pagination;
using HomeEnergyApi.Dtos;
using HomeEnergyApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net.Http.Headers;
public class HomeAdminControllerTest : UserAcceptanceTest
{
    private readonly HomeDto _homeDto;

    public HomeAdminControllerTest(WebApplicationFactory<Program> factory): base(factory, "Admin")
    {
        var result = new PaginatedResult<Home>();
        result.Items = new List<Home>();
        _homeDto = new HomeDto
        {
            OwnerLastName = "Testy",
            StreetAddress = "49 Test St",
            City = "Test City",
            MonthlyElectricUsage = 1234,
        };
    }

    [Fact]
    public async Task ShouldCreateHome_WhenGivenValidHome()
    {
        var response = await _client.PostAsJsonAsync("/admin/Homes", _homeDto);
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdHome = await response.Content.ReadFromJsonAsync<Home>();
        Assert.NotNull(createdHome);
        Assert.True(createdHome.Id > 0);
        Assert.Equal(_homeDto.OwnerLastName, createdHome.OwnerLastName);
        Assert.Equal(_homeDto.StreetAddress, createdHome.StreetAddress);
        Assert.Equal(_homeDto.City, createdHome.City);
        Assert.Equal(_homeDto.MonthlyElectricUsage, createdHome.HomeUsageData.MonthlyElectricUsage);
    }

    [Fact]
    public async Task ShouldNotAllow_WhenUserIsNotAdmin()
    {
        var token = await JwtHelper.GenerateTokenAsync(_client, "NotAdmin");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/admin/Homes", _homeDto);
        
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ShouldNotCreateHome_WhenHomeStreetAddressDoesNotContainDigit()
    {
        _homeDto.StreetAddress = "NoNumber St";

        var response = await _client.PostAsJsonAsync("/admin/Homes", _homeDto);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ShouldNotCreateHome_WhenHomeStreetAddressIsTooLong()
    {
        _homeDto.StreetAddress = new string('A', 65);

        var response = await _client.PostAsJsonAsync("/admin/Homes", _homeDto);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    //Fact attribute?
    [Fact]
    public async Task ShouldDeleteHome_WhenGivenValidHomeId()
    {
        var response = await _client.PostAsJsonAsync("/admin/Homes", _homeDto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var home = await response.Content.ReadFromJsonAsync<Home>();
        Assert.NotNull(home);

        var finalResponse = await _client.DeleteAsync("/admin/Homes/" + home.Id);
        Assert.Equal(HttpStatusCode.OK, finalResponse.StatusCode);
        var deletedHome = await finalResponse.Content.ReadFromJsonAsync<Home>();
        Assert.NotNull(deletedHome);
        Assert.Equal(home.Id, deletedHome.Id);

    }

    [Fact]
    public async Task ShouldNotDeleteHome_WhenGivenInvalidHomeId()
    {
        var response = await _client.PostAsJsonAsync("/admin/Homes", _homeDto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var home = await response.Content.ReadFromJsonAsync<Home>();
        Assert.NotNull(home);

        var invalidId = 100 + home.Id;

        var finalResponse = await _client.DeleteAsync("/admin/Homes/" + invalidId);
        Assert.Equal(HttpStatusCode.NotFound, finalResponse.StatusCode);

    }
}