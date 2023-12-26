using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Enums;

namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration
{
    public class EventClientConfiguration
    {
        public string ApiName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<string> Hostnames { get; set; } = [];
        public string ExchangeName { get; set; } = string.Empty;
        public string ExchangeType { get; set; } = string.Empty;
        public int Port { get; set; }
        public string RouteKey { get; set; } = string.Empty;
        public RabbitMQDeliveryMode DeliveryMode { get; set; } = RabbitMQDeliveryMode.Durable;

        public EventClientConfiguration CopyFrom(EventClientConfiguration config)
        {
            ApiName = config.ApiName;
            Username = config.Username;
            Password = config.Password;
            Hostnames = config.Hostnames;
            ExchangeName = config.ExchangeName;
            ExchangeType = config.ExchangeType;
            Port = config.Port;
            DeliveryMode = config.DeliveryMode;
            RouteKey = config.RouteKey;
            return this;
        }
    }
}
