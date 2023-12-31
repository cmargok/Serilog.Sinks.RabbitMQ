using Microsoft.Extensions.Logging;

namespace Serilog.Sinks.RabbitMQ.Publisher.Expose.EventLogBus.Client
{
    /// <summary>
    /// Represents an event logger that implements the <see cref="IEventLogger"/> interface.
    /// </summary>
    /// <param name="logger">The logger instance used for logging.</param>
    public class EventLogger(ILogger<EventLogger> logger) : IEventLogger
    {
        private readonly ILogger<EventLogger> _logger = logger;

#pragma warning disable CA2254 // Template should be a static expression
        /// <summary>
        /// Logs informational messages.
        /// </summary>
        /// <param name="message">The informational message to be logged.</param>
        public void LoggingInformation(string message)
            => _logger.LogInformation(message);


        /// <summary>
        /// Logs warning messages.
        /// </summary>
        /// <param name="message">The warning message to be logged.</param>
        public void LoggingWarning(string message)
            => _logger.LogWarning(message);


        /// <summary>
        /// Logs error messages along with the associated exception details.
        /// </summary>
        /// <param name="ex">The exception object to be logged.</param>
        /// <param name="message">The error message to be logged.</param>
        public void LoggingError(Exception ex, string message)

            => _logger.LogError(ex, message, null!);
#pragma warning restore CA2254 // Template should be a static expression
    }
}
