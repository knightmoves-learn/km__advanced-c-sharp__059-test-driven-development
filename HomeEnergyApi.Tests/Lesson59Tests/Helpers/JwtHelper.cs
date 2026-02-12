using System.Net;
using System.Net.Http.Json;
using HomeEnergyApi.Dtos;

public class JwtHelper{
    public static  async Task<string> GenerateTokenAsync(HttpClient client, string role)
    {
        var guid = Guid.NewGuid().ToString();
        var userDto = new UserDtoV1
        {
                Username = "TestUser" + guid,
                Password = "TestPassword" + guid,
                Role = role,
                HomeStreetAddress = "123 Main St"
        };

        var initialSetupResponse = await client.PostAsJsonAsync("/v1/authentication/register", userDto);
        Assert.Equal(HttpStatusCode.OK, initialSetupResponse.StatusCode);

        var response = await client.PostAsJsonAsync("/v1/authentication/token", userDto);
        var content = await response.Content.ReadFromJsonAsync<TokenResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        return content?.Token;
    }
}

class TokenResponse
{
    public string Token { get; set; }
}