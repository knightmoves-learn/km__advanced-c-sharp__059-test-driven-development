using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using HomeEnergyApi.Dtos;
public class AuthenticationV1ControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly UserDtoV1 _userDto;
    
    public AuthenticationV1ControllerTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        var guid = Guid.NewGuid().ToString();
        _userDto = new UserDtoV1
        {
                Username = "TestUser" + guid,
                Password = "TestPassword" + guid,
                Role = "Admin",
                HomeStreetAddress = "123 Test St",
        };

        _client.DefaultRequestHeaders.Add("X-API-Key", ConfigHelper.LookupSecret("ApiKey"));
    }

    [Fact]
    public async Task ShouldCreateUser_WhenValidRequest()
    {
        var response = await _client.PostAsJsonAsync("/v1/authentication/register", _userDto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("User registered successfully.", content);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenUsernameAlreadyExists()
    {
        var initialSetupResponse = await _client.PostAsJsonAsync("/v1/authentication/register", _userDto);
        Assert.Equal(HttpStatusCode.OK, initialSetupResponse.StatusCode);

        var response = await _client.PostAsJsonAsync("/v1/authentication/register", _userDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Username is already taken.", content);
    }

    [Fact]
    public async Task ShouldReturnOk_WhenValidCredentialsProvided()
    {
        var initialSetupResponse = await _client.PostAsJsonAsync("/v1/authentication/register", _userDto);
        Assert.Equal(HttpStatusCode.OK, initialSetupResponse.StatusCode);

        var response = await _client.PostAsJsonAsync("/v1/authentication/token", _userDto);
        var content = await response.Content.ReadFromJsonAsync<TokenResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content.Token);
        Assert.Equal(407, content.Token.Length);
    }

    [Fact]
    public async Task ShouldReturnUnauthorized_WhenInvalidCredentialsProvided()
    {
        var response = await _client.PostAsJsonAsync("/v1/authentication/token", _userDto);
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("Invalid username or password.", content);
    }
}