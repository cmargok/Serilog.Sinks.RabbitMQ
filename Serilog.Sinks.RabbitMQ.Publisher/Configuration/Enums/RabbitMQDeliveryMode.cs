namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum RabbitMQDeliveryMode : byte
    {
        /// <summary>
        /// 
        /// </summary>
        NonDurable = 1,

        /// <summary>
        /// 
        /// </summary>
        Durable = 2
    }
}
