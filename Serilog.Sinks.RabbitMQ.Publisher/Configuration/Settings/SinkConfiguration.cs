using Serilog.Formatting;
using Serilog.Formatting.Json;
namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings
{
    internal class BatchSinkConfiguration : OpenSinkConfiguration
    {
        public ITextFormatter TextFormatter { get; set; } = new JsonFormatter();

    }
}
