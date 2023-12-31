using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Enums;

namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class QueueConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public string QueueName { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public RabbitMQDeliveryMode DeliveryMode { get; set; } = RabbitMQDeliveryMode.Durable;

    }


}
