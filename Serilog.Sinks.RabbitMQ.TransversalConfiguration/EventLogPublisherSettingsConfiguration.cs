﻿using Microsoft.Extensions.Configuration;
using Serilog.Sinks.RabbitMQ.TransversalConfiguration.Enums;
using Serilog.Sinks.RabbitMQ.TransversalConfiguration.Tools;
namespace Serilog.Sinks.RabbitMQ.TransversalConfiguration
{
    public static class EventLogPublisherSettingsConfiguration
    {
        private static readonly string _errorMessage = "Settings json section was no configured correctly";

        public static EventClientConfiguration GetEventLogPublisherSettings(this IConfiguration config, RabbitMQDeliveryMode DeliveryMode = RabbitMQDeliveryMode.Durable)
        {
            string KeySection = "EventLogPublisher";

            EventClientConfiguration Settings = config.GetSection(KeySection).Get<EventClientConfiguration>()!;

            Settings.DeliveryMode = DeliveryMode;

            if (Settings is null) ToolServant.ThrownByArgument(nameof(EventClientConfiguration), _errorMessage);

            return Settings!;
        }
    }
}