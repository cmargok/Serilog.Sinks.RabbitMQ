using Serilog.Configuration;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings;

namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools.Validator
{

    internal class SettingsValidator
    {
        public static void Validate(EventClientConfiguration clientConfiguration)
        {
            #region EventClientConfiguration guards
            if (string.IsNullOrEmpty(value: clientConfiguration.Username))
                Throwner(
                    paramName: nameof(clientConfiguration.Username), 
                    message: "username cannot be 'null' or and empty string."
                    );

            if (clientConfiguration.Password is null)
                Throwner(
                    paramName: nameof(clientConfiguration.Password), 
                    message: "password cannot be 'null'. Specify an empty string if password is empty."
                    );

            if (clientConfiguration.Hostnames.Any(predicate: string.IsNullOrEmpty))
                Throwner(
                    paramName: nameof(clientConfiguration.Hostnames), 
                    message: "hostnames cannot be empty, specify at least one hostname"
                    );

            if (clientConfiguration.Port <= 0 || clientConfiguration.Port > 65535)
                Throwner(
                    paramName: nameof(clientConfiguration.Port), 
                    message: "port must be in a valid range (1 and 65535)"
                    );

            clientConfiguration.ApiName = string.IsNullOrEmpty(value: clientConfiguration.ApiName)
                ? "EventLogBus"
                : clientConfiguration.ApiName;

            clientConfiguration.Exchange.ExchangeName = string.IsNullOrEmpty(value: clientConfiguration.Exchange.ExchangeName)
                ? "LogExchange"
                : clientConfiguration.Exchange.ExchangeName;

            clientConfiguration.Exchange.ExchangeType = string.IsNullOrEmpty(value: clientConfiguration.Exchange.ExchangeType)
                ? "Direct"
                : clientConfiguration.Exchange.ExchangeType;

            clientConfiguration.Exchange.RouteKey = string.IsNullOrEmpty(value: clientConfiguration.Exchange.RouteKey)
                ? "ApplicationLogs"
                : clientConfiguration.Exchange.RouteKey;
            #endregion
        }
        public static void SinkSettingsValidator(BatchSinkConfiguration sinkConfiguration)
        {
            #region EventClientConfiguration guards
            if (sinkConfiguration.TextFormatter is null)
                Throwner(
                    paramName: nameof(sinkConfiguration.TextFormatter), 
                    message: "TextFormatter cannot be null"
                    );


            if (sinkConfiguration.BatchPostingLimit <= 0 || sinkConfiguration.BatchPostingLimit > 5000)
                Throwner(
                    paramName: nameof(sinkConfiguration.BatchPostingLimit), 
                    message: "BatchPostingLimit must be in a valid range (1 and 5000)"
                    );


            if (sinkConfiguration.Period <= TimeSpan.FromMilliseconds(10))
                Throwner(
                    paramName: nameof(sinkConfiguration.Period), message: "Period must be major than 10 miliseconds"
                    );


            if (sinkConfiguration.QueueLimit <= 0 || sinkConfiguration.QueueLimit > 5000)
                Throwner(
                    paramName: nameof(sinkConfiguration.QueueLimit), 
                    message: "QueueLimit must be in a valid range (1 and 5000)"
                    );
            #endregion
        }

        private static void Throwner(string paramName, string message)
            => ToolServant.ThrownByArgument(paramName: paramName,message: message);
    }



}
