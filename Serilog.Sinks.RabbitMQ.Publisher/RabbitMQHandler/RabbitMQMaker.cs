using RabbitMQ.Client;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Enums;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings;

namespace Serilog.Sinks.RabbitMQ.Publisher.RabbitMQHandler
{
    internal class RabbitMQMaker
    {
        public static void CreaterExchangeAndQueue(EventClientConfiguration eventConfig, QueueConfiguration queueSettings)
        {
            var factories = eventConfig.Hostnames.Select(host => new ConnectionFactory()
            {
                HostName = host,
                UserName = eventConfig.Username,
                Password = eventConfig.Password,
                Port = eventConfig.Port
            }).AsEnumerable();

            bool isExchangeDurable = eventConfig.Exchange.DeliveryMode == RabbitMQDeliveryMode.Durable;
            bool isQueueDurable = queueSettings.DeliveryMode == RabbitMQDeliveryMode.Durable;


            foreach (var factory in factories)
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                // Declara el exchange
                channel.ExchangeDeclare(
                    exchange: eventConfig.Exchange.ExchangeName, 
                    type: eventConfig.Exchange.ExchangeType, 
                    durable: isExchangeDurable, 
                    autoDelete: false, 
                    arguments: null); ;

                // Declara la cola y la une al exchange con una routing key                    
                channel.QueueDeclare(
                    queue: queueSettings.QueueName, 
                    durable: isQueueDurable, 
                    exclusive: false, 
                    autoDelete: false, 
                    arguments: null);

                // bind the exchange and the que trough the routingkey
                channel.QueueBind(
                    queue: queueSettings.QueueName, 
                    exchange: eventConfig.Exchange.ExchangeName, 
                    routingKey: eventConfig.Exchange.RouteKey);
            };


        }

    }
}
