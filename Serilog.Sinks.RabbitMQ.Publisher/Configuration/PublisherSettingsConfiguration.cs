using Microsoft.Extensions.Configuration;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Enums;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools;

namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration
{
    /// <summary>
    /// Class to bind settings frrom jsonSettings
    /// </summary>
    public static class PublisherSettingsConfiguration
    {
        private static readonly string _errorMessage = "Settings json section was no configured correctly";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="DeliveryMode"></param>
        /// <returns></returns>
        public static EventClientConfiguration GetEventLogPublisherSettings(this IConfiguration config, RabbitMQDeliveryMode DeliveryMode = RabbitMQDeliveryMode.Durable)
        {
            string KeySection = "EventLogPublisher";

            EventClientConfiguration Settings = config.GetSection(key: KeySection).Get<EventClientConfiguration>()!;

            Settings.Exchange.DeliveryMode = DeliveryMode;

            if (Settings is null) ToolServant.ThrownByArgument(paramName: nameof(EventClientConfiguration), message: _errorMessage);

            return Settings!;
        }

        //TODO 
        /*
         * implemenent reading parameters from enviroment  to use this in docker 
         * 
         */
    }
}
