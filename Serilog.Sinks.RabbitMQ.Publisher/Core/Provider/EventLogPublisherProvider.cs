﻿using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration;

namespace Serilog.Sinks.RabbitMQ.Publisher.Core.Provider
{
    public static class EventLogPublisherProvider
    {
        public static PublisherSlave AddEventLogPublisher(this IServiceCollection services, Action<EventClientConfiguration, OpenSinkConfiguration> settings)
        {
            ArgumentNullException.ThrowIfNull(services);

            var eventConfig = new EventClientConfiguration();
            var sinkConfig = new OpenSinkConfiguration();

            settings(eventConfig, sinkConfig);

            return PublisherSlave.CreateClient(services, eventConfig, sinkConfig);
        }
    }
}