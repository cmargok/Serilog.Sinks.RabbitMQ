namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools
{
    internal static class ToolServant
    {
        public static void ThrownByArgument(string paramName, string message)
            => throw new ArgumentOutOfRangeException(paramName: paramName, message: message);
    }
}
