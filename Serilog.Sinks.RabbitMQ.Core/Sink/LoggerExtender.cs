using Serilog.Configuration;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Sinks.RabbitMQ.Core.Sink.SinkExecutor;
using Serilog.Sinks.RabbitMQ.TransversalConfiguration;
using Serilog.Sinks.RabbitMQ.TransversalConfiguration.Tools;

namespace Serilog.Sinks.RabbitMQ.Core.Sink
{
    public static class LoggerExtender
    {
        public static LoggerConfiguration RabbitMQEventLog(this LoggerSinkConfiguration loggerConfiguration, Action<EventClientConfiguration, BatchSinkConfiguration> configure)
        {
            EventClientConfiguration clientConfiguration = new();
            BatchSinkConfiguration sinkConfiguration = new();

            configure(clientConfiguration, sinkConfiguration);

            CheckAndFill(loggerConfiguration, clientConfiguration, sinkConfiguration);

            var batchingOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = sinkConfiguration.BatchPostingLimit,
                Period = sinkConfiguration.Period,
                EagerlyEmitFirstEvent = true,
                QueueLimit = sinkConfiguration.QueueLimit,

            };

            var RabbitMQSink = new PeriodicBatchingSink(new RabbitMQBatchSink(clientConfiguration, sinkConfiguration.TextFormatter), batchingOptions);
            return loggerConfiguration.Sink(RabbitMQSink, sinkConfiguration.LogMinimumLevel);

        }

        private static void CheckAndFill(LoggerSinkConfiguration loggerConfiguration,
            EventClientConfiguration clientConfiguration,
            BatchSinkConfiguration sinkConfiguration
            )
        {

            #region LoggerSinkConfiguration guards
            ArgumentNullException.ThrowIfNull(loggerConfiguration);
            #endregion


            #region EventClientConfiguration guards

            if (string.IsNullOrEmpty(clientConfiguration.Username))
                Throwner(nameof(clientConfiguration.Username), "username cannot be 'null' or and empty string.");

            if (clientConfiguration.Password is null)
                Throwner(nameof(clientConfiguration.Password), "password cannot be 'null'. Specify an empty string if password is empty.");

            if (clientConfiguration.Hostnames.Count == 0)
                Throwner(nameof(clientConfiguration.Hostnames), "hostnames cannot be empty, specify at least one hostname");

            if (clientConfiguration.Port <= 0 || clientConfiguration.Port > 65535)
                Throwner(nameof(clientConfiguration.Port), "port must be in a valid range (1 and 65535)");

            clientConfiguration.ApiName = string.IsNullOrEmpty(clientConfiguration.ApiName) ? "EventLogBus" : clientConfiguration.ApiName;

            clientConfiguration.ExchangeName = string.IsNullOrEmpty(clientConfiguration.ExchangeName) ? "LogExchange" : clientConfiguration.ExchangeName;

            clientConfiguration.ExchangeType = string.IsNullOrEmpty(clientConfiguration.ExchangeType) ? "Direct" : clientConfiguration.ExchangeType;

            clientConfiguration.RouteKey = string.IsNullOrEmpty(clientConfiguration.RouteKey) ? "ApplicationLogs" : clientConfiguration.RouteKey;
            #endregion


            #region EventClientConfiguration guards
            if (sinkConfiguration.TextFormatter is null)
                Throwner(nameof(sinkConfiguration.TextFormatter), "TextFormatter cannot be null");


            if (sinkConfiguration.BatchPostingLimit <= 0 || sinkConfiguration.BatchPostingLimit > 5000)
                Throwner(nameof(sinkConfiguration.BatchPostingLimit), "BatchPostingLimit must be in a valid range (1 and 5000)");


            if (sinkConfiguration.Period <= TimeSpan.FromMilliseconds(10))
                Throwner(nameof(sinkConfiguration.Period), "Period must be major than 10 miliseconds");


            if (sinkConfiguration.QueueLimit <= 0 || sinkConfiguration.QueueLimit > 5000)
                Throwner(nameof(sinkConfiguration.QueueLimit), "QueueLimit must be in a valid range (1 and 5000)");
            #endregion
        }

        private static void Throwner(string paramName, string message)
            => ToolServant.ThrownByArgument(paramName, message);


    }
}
