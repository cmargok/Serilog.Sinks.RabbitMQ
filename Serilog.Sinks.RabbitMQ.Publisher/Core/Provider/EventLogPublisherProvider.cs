using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools.Validator;

namespace Serilog.Sinks.RabbitMQ.Publisher.Core.Provider
{

    /// <summary>
    /// Register the event log sink to services collection
    /// </summary>
    public static partial class PublisherProvider
    {
        /// <summary>
        /// Register the event log sink to services collection
        /// </summary>
        /// <param name="services">Iservice collection </param>
        /// <param name="settings">Configuration to set behaviour inside the event log</param>
        /// <returns></returns>
        public static PublisherSlave AddEventLogPublisher(this IServiceCollection services, Action<EventClientConfiguration, OpenSinkConfiguration> settings)
        {
            ArgumentNullException.ThrowIfNull(argument: services);

            var eventConfig = new EventClientConfiguration();
            var sinkConfig = new OpenSinkConfiguration();

            settings(eventConfig, sinkConfig);

            SettingsValidator.Validate(clientConfiguration: eventConfig);

            return PublisherSlave.CreateClient(services: services, eventConfig: eventConfig, sinkConfig: sinkConfig);
        }
    }

      
}
