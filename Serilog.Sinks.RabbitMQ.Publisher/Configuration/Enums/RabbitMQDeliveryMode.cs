namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Enums
{
    public enum RabbitMQDeliveryMode : byte
    {
        NonDurable = 1,
        Durable = 2
    }
}
