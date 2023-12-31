using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings;
using Serilog.Sinks.RabbitMQ.Publisher.Core.Sink;
using Serilog.Sinks.RabbitMQ.Publisher.Expose.EventLogBus.Client;
using Serilog.Sinks.RabbitMQ.Publisher.RabbitMQHandler;

namespace Serilog.Sinks.RabbitMQ.Publisher.Core.Provider
{
    /// <summary>
    /// handles the configuration to register the event client and the creation of the queue and the xchange
    /// </summary>
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

            ArgumentNullException.ThrowIfNull(argument: eventConfig);
            ArgumentNullException.ThrowIfNull(argument: sinkConfig);

            _eventSettings = eventConfig;
            _sinkConfiguration = sinkConfig;
            _loggerConfiguration = new LoggerConfiguration();
        }

        /// <summary>
        /// Creatas a client of publisherClient
        /// </summary>
        /// <param name="services"></param>
        /// <param name="eventConfig"></param>
        /// <param name="sinkConfig"></param>
        /// <returns></returns>
        public static PublisherSlave CreateClient(IServiceCollection services, EventClientConfiguration eventConfig, OpenSinkConfiguration sinkConfig)
            => new(services, eventConfig, sinkConfig);


        /// <summary>
        /// Register the eventlog client 
        /// </summary>       
        /// <returns></returns>
        public PublisherSlave RegisterPublisher()
        {
            return Register(enableRabbitMQClient: false, UseDefaultLogger: true);
        }


        /// <summary>
        /// Register the eventlog client with option to adding console log
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="enableRabbitMQClientLogger"></param>
        /// <param name="UseDefaultLogger"></param>
        /// <returns></returns>
        public PublisherSlave RegisterPublisher(LogEventLevel logLevel = LogEventLevel.Debug, bool enableRabbitMQClientLogger = false, bool UseDefaultLogger = true)
        {

            _loggerConfiguration.WriteTo.Logger(cl => cl
                .Filter.ByIncludingOnly(e => e.Level > logLevel)
                .WriteTo.Console());

            return Register(enableRabbitMQClient: enableRabbitMQClientLogger, UseDefaultLogger: UseDefaultLogger);
        }




        /// <summary>
        /// Register the eventlog client
        /// </summary>
        /// <param name="loggerConfig"></param>
        /// <param name="enableRabbitMQClientLogger"></param>
        /// <param name="UseDefaultLogger"></param>
        /// <returns></returns>
        public PublisherSlave RegisterPublisher(Action<LoggerConfiguration> loggerConfig = null!, bool enableRabbitMQClientLogger = false, bool UseDefaultLogger = true)
        {

            if (loggerConfig != null)
                loggerConfig?.Invoke(obj: _loggerConfiguration);


            return Register(enableRabbitMQClient: enableRabbitMQClientLogger, UseDefaultLogger: UseDefaultLogger);
        }

     

      


        /// <summary>
        /// It createas a new exchange and  queue, they will be binding by the routing key
        /// if the exchangeor queue already exists, this wont do nothing
        /// </summary>
        /// <param name="queueSettings">settings configuration to set queue name and queue durability</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">if the queueSettings object is not defined or the queuename is empoty or null</exception>
        public PublisherSlave CreateExchangeAndQueue(Action<QueueConfiguration> queueSettings)
        {
            ArgumentNullException.ThrowIfNull(argument: queueSettings);
            var queue = new QueueConfiguration();

            queueSettings.Invoke(obj: queue);

            if (string.IsNullOrEmpty(value: queue!.QueueName))
                throw new ArgumentException(nameof(queue.QueueName));

            RabbitMQMaker.CreaterExchangeAndQueue(eventConfig: _eventSettings!,queueSettings: queue);

            return this;
        }


        private PublisherSlave Register(bool enableRabbitMQClient = false, bool UseDefaultLogger = true)
        {
            if (!enableRabbitMQClient)
            {
                _loggerConfiguration.WriteTo.RabbitMQEventLog((clientConfiguration, sinkConfiguration) =>
                {
                    clientConfiguration.Clone(config: _eventSettings!);
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
