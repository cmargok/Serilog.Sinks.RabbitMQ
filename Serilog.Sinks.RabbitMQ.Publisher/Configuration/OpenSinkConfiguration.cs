using Serilog.Events;

namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration
{
    public class OpenSinkConfiguration
    {
        public int BatchPostingLimit { get; set; } = 50;
        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(20);
        public int QueueLimit { get; set; } = 50;
        public LogEventLevel LogMinimumLevel { get; set; } = LogEventLevel.Error;

    }
}
