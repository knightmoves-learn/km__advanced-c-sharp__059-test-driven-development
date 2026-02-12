using HomeEnergyApi.Services;
using HomeEnergyApi.Wrapper;

public class StubRateLimitingService : RateLimitingService
{
    private readonly bool _isRequestAllowed;

    public StubRateLimitingService(bool isRequestAllowed) 
        : base(new DateTimeWrapper())
    {
        _isRequestAllowed = isRequestAllowed;
    }

    public override bool IsRequestAllowed(string clientKey)
    {
        return _isRequestAllowed;
    }
}