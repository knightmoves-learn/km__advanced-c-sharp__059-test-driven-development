using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;

public class UserAcceptanceTest : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    protected readonly HttpClient _client;
    private readonly string _role;

    public UserAcceptanceTest(WebApplicationFactory<Program> factory, string role)
    {
        _client = factory.CreateClient();

        _client.DefaultRequestHeaders.Add("X-API-Key", ConfigHelper.LookupSecret("ApiKey"));

        _role = role;
    }

    public async Task InitializeAsync()
    {
        var token = await JwtHelper.GenerateTokenAsync(_client, _role);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}