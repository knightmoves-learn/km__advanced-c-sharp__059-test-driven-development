using HomeEnergyApi.Middleware;
using HomeEnergyApi.Services;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;

public class RateLimitingMiddlewareTest
{
    private readonly StubLogger<RateLimitingMiddleware> _stubLogger;
    private readonly DefaultHttpContext _expectedHttpContext;
    private readonly StubRateLimitingService _underRateLimitService;
    private readonly StubRateLimitingService _overRateLimitService;
    private HttpContext? _actualContext = null;
    private RequestDelegate _stubRequestDelegate;
    
    // This constructor will be called before each test
    public RateLimitingMiddlewareTest()
    {
        _stubLogger = new StubLogger<RateLimitingMiddleware>();
        _expectedHttpContext = new DefaultHttpContext();
        _expectedHttpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        _expectedHttpContext.Response.Body = new MemoryStream();

        _underRateLimitService = new StubRateLimitingService(true);
        _overRateLimitService = new StubRateLimitingService(false);
        
        _stubRequestDelegate = async (HttpContext context) => _actualContext = context;
    }

    [Fact]
    public async Task ShouldCallNextMiddleware_WhenRateLimitNotExceeded()
    {
        // Arrange
        var middleware = new RateLimitingMiddleware(_stubRequestDelegate, _underRateLimitService, _stubLogger);

        // Act
        await middleware.InvokeAsync(_expectedHttpContext);

        // Assert
        _expectedHttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_expectedHttpContext.Response.Body, Encoding.UTF8).ReadToEndAsync();
        Assert.Equal("", responseBody);

        Assert.Same(_expectedHttpContext, _actualContext);

        Assert.Equal(StatusCodes.Status200OK, _expectedHttpContext.Response.StatusCode);

        Assert.Equal(1, _stubLogger.LoggedDebugMessages.Count);
        Assert.Equal("RateLimitingMiddleware Started", _stubLogger.LoggedDebugMessages[0]);
    }

    [Fact]
    public async Task ShouldReturn429_WhenRateLimitExceeded()
    {
        // Arrange
        var middleware = new RateLimitingMiddleware(_stubRequestDelegate, _overRateLimitService, _stubLogger);

        // Act
        await middleware.InvokeAsync(_expectedHttpContext);

        // Assert
        _expectedHttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_expectedHttpContext.Response.Body, Encoding.UTF8).ReadToEndAsync();
        Assert.Equal("Slow down! Too many requests.", responseBody);

        Assert.Null(_actualContext);

        Assert.Equal(StatusCodes.Status429TooManyRequests, _expectedHttpContext.Response.StatusCode);
        
        Assert.Single(_stubLogger.LoggedDebugMessages);
        Assert.Equal("RateLimitingMiddleware Started", _stubLogger.LoggedDebugMessages[0]);
    }
}