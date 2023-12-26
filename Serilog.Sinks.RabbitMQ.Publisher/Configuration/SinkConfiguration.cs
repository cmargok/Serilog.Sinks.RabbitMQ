using Serilog.Formatting;
using Serilog.Formatting.Json;
namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration
{
    public class BatchSinkConfiguration : OpenSinkConfiguration
    {
        public ITextFormatter TextFormatter { get; set; } = new JsonFormatter();

    }
}
