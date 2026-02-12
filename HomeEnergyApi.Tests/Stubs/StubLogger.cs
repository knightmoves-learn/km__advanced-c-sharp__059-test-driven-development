using Microsoft.Extensions.Logging;

class StubLogger<T> : ILogger<T>
{
    public List<string> LoggedInfoMessages { get; } = new List<string>();
    public List<string> LoggedDebugMessages { get; } = new List<string>(); 

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (logLevel == LogLevel.Information)
        {
            LoggedInfoMessages.Add(formatter(state, exception));
        }        
        if (logLevel == LogLevel.Debug)
        {
            LoggedDebugMessages.Add(formatter(state, exception));
        }
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }
}