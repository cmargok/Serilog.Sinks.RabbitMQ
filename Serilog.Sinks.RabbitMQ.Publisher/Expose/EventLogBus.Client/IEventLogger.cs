namespace Serilog.Sinks.RabbitMQ.Publisher.Expose.EventLogBus.Client
{
    public interface IEventLogger
    {
        public void LoggingInformation(string message);
        public void LoggingWarning(string message);
        public void LoggingError(Exception ex, string message);
    }
}
