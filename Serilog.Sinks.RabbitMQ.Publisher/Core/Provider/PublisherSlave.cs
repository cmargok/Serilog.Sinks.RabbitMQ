using Microsoft.Extensions.DependencyInjection;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration;
using Serilog.Sinks.RabbitMQ.Publisher.Core.Sink;
using Serilog.Sinks.RabbitMQ.Publisher.Expose.EventLogBus.Client;

namespace Serilog.Sinks.RabbitMQ.Publisher.Core.Provider
{

    public class PublisherSlave
    {
        private readonly IServiceCollection _services;
        private readonly EventClientConfiguration? _eventSettings;
        private readonly OpenSinkConfiguration? _sinkConfiguration;

#pragma warning disable IDE0044 // Add readonly modifier
        private LoggerConfiguration _loggerConfiguration;
#pragma warning restore IDE0044 // Add readonly modifier




        private PublisherSlave(IServiceCollection services, EventClientConfiguration eventConfig, OpenSinkConfiguration sinkConfig)
        {
            _services = services;

            ArgumentNullException.ThrowIfNull(eventConfig);
            ArgumentNullException.ThrowIfNull(sinkConfig);

            _eventSettings = eventConfig;
            _sinkConfiguration = sinkConfig;
            _loggerConfiguration = new LoggerConfiguration();
        }

        public static PublisherSlave CreateClient(IServiceCollection services, EventClientConfiguration eventConfig, OpenSinkConfiguration sinkConfig)
            => new(services, eventConfig, sinkConfig);






        public PublisherSlave RegisterPublisher(Action<LoggerConfiguration> loggerConfig = null!, bool SleepRabbitMQClientLogger = false, bool UseDefaultLogger = true)
        {

            if (loggerConfig != null)
                loggerConfig?.Invoke(_loggerConfiguration);

            if (!SleepRabbitMQClientLogger)
            {
                _loggerConfiguration.WriteTo.RabbitMQEventLog((clientConfiguration, sinkConfiguration) =>
                {
                    clientConfiguration.CopyFrom(_eventSettings!);
                    sinkConfiguration.BatchPostingLimit = _sinkConfiguration!.BatchPostingLimit;
                    sinkConfiguration.Period = _sinkConfiguration.Period;
                    sinkConfiguration.QueueLimit = _sinkConfiguration.QueueLimit;
                    sinkConfiguration.TextFormatter = new JsonFormatter();
                    sinkConfiguration.LogMinimumLevel = _sinkConfiguration.LogMinimumLevel;
                });
            }


            Log.Logger = _loggerConfiguration.CreateLogger();

            if (UseDefaultLogger)
                _services.AddSingleton<IEventLogger, EventLogger>();
            return this;
        }



    }
}
