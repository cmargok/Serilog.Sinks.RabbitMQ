using Serilog.Events;

namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings
{

    /// <summary>
    /// Sink configuration object to configure batch sink
    /// </summary>
    public class OpenSinkConfiguration
    {
        /// <summary>
        /// Limits the queue of logs to sent
        /// </summary>
        public int BatchPostingLimit { get; set; } = 50;

        /// <summary>
        /// sets the time between sending and sending
        /// </summary>
        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(20);

        /// <summary>
        /// set the maximum limit for saving in the queue
        /// </summary>
        public int QueueLimit { get; set; } = 50;

        /// <summary>
        /// sets the minimum log level
        /// </summary>
        public LogEventLevel LogMinimumLevel { get; set; } = LogEventLevel.Error;

    }
}
