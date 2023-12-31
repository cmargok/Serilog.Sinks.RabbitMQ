namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Entities
{
    internal class EventTo
    {
        public string ApiLog { get; set; } = string.Empty;

        public string ApiLogFrom { get; set; } = string.Empty;
    }
}
