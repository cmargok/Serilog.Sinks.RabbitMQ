using Microsoft.Extensions.Logging;

namespace Serilog.Sinks.RabbitMQ.Publisher.Expose.EventLogBus.Client
{
    public class EventLogger(ILogger<EventLogger> logger) : IEventLogger
    {
        private readonly ILogger<EventLogger> _logger = logger;
#pragma warning disable CA2254 // Template should be a static expression
        public void LoggingInformation(string message)
            => _logger.LogInformation(message);

        public void LoggingWarning(string message)
            => _logger.LogWarning(message);

        public void LoggingError(Exception ex, string message)

            => _logger.LogError(ex, message, null!);
#pragma warning restore CA2254 // Template should be a static expression
    }
}
