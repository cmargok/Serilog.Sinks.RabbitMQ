namespace Serilog.Sinks.RabbitMQ.TransversalConfiguration.Entities
{
    public class EventTo
    {
        public string ApiLog { get; set; } = string.Empty;

        public string ApiLogFrom { get; set; } = string.Empty;
    }
}
