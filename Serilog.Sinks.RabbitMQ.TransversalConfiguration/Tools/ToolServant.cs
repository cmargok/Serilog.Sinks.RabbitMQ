namespace Serilog.Sinks.RabbitMQ.TransversalConfiguration.Tools
{
    public static class ToolServant
    {
        public static void ThrownByArgument(string paramName, string message)
            => throw new ArgumentOutOfRangeException(paramName, message);
    }
}
