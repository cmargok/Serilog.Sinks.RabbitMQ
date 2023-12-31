using Serilog.Configuration;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools.Validator;
using Serilog.Sinks.RabbitMQ.Publisher.Core.Sink.SinkExecutor;

namespace Serilog.Sinks.RabbitMQ.Publisher.Core.Sink
{
    internal static class LoggerExtender
    {
        public static LoggerConfiguration RabbitMQEventLog(this LoggerSinkConfiguration loggerConfiguration, Action<EventClientConfiguration, BatchSinkConfiguration> configure)
        {
            EventClientConfiguration clientConfiguration = new();
            BatchSinkConfiguration sinkConfiguration = new();

            configure(clientConfiguration, sinkConfiguration);


            #region LoggerSinkConfiguration guards
            ArgumentNullException.ThrowIfNull(argument: loggerConfiguration);
            #endregion

            SettingsValidator.SinkSettingsValidator(sinkConfiguration: sinkConfiguration);

            var batchingOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = sinkConfiguration.BatchPostingLimit,
                Period = sinkConfiguration.Period,
                EagerlyEmitFirstEvent = true,
                QueueLimit = sinkConfiguration.QueueLimit,

            };

            var RabbitMQSink = new PeriodicBatchingSink( 
                batchedSink: new RabbitMQBatchSink(clientConfiguration, sinkConfiguration.TextFormatter), 
                options: batchingOptions);

            return loggerConfiguration.Sink(
                logEventSink: RabbitMQSink, 
                restrictedToMinimumLevel: sinkConfiguration.LogMinimumLevel
                );
        }        
    }    
}
