namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools
{
    public class EventLogBusException(string message, IEnumerable<Exception> exceptions) : Exception(message)
    {
        public readonly IEnumerable<Exception> _innerExceptions = exceptions;
    }
}
