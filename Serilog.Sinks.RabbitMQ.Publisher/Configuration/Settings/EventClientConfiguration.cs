using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Enums;

namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class EventClientConfiguration 
    {
        /// <summary>
        /// 
        /// </summary>
        public string ApiName { get; set; } = string.Empty;


        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; } = string.Empty;


        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; } = string.Empty;


        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public List<string> Hostnames { get; set; } = [];


        /// <summary>
        /// 
        /// </summary>
        public ExchangeConfiguration Exchange { get; set; } = new();


        /// <summary>
        /// 
        /// </summary>
        public EventClientConfiguration Clone(EventClientConfiguration config)
        {
            ApiName = config.ApiName;
            Hostnames = config.Hostnames.Select(host => host).ToList();
            Username = config.Username;
            Password = config.Password;
            Port = config.Port;
            Exchange.ExchangeName = config.Exchange.ExchangeName;
            Exchange.ExchangeType = config.Exchange.ExchangeType;
            Exchange.DeliveryMode = config.Exchange.DeliveryMode;
            Exchange.RouteKey = config.Exchange.RouteKey;
            return this;
        }


        
        
       
    }



    /// <summary>
    /// 
    /// </summary>
    public class ExchangeConfiguration
    {

        /// <summary>
        /// 
        /// </summary>
        public string ExchangeName { get; set; } = string.Empty;


        /// <summary>
        /// 
        /// </summary>
        public string ExchangeType { get; set; } = string.Empty;


        /// <summary>
        /// 
        /// </summary>
        public string RouteKey { get; set; } = string.Empty;


        /// <summary>
        /// 
        /// </summary>
        public RabbitMQDeliveryMode DeliveryMode { get; set; } = RabbitMQDeliveryMode.Durable;

    }


}
