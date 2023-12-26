namespace EventLogBus.Client
{
    public interface IEventLogger
    {
        public void LoggingWarning(string message);
        public void LoggingInformation(string message);
        public void LoggingError(Exception ex, string message);
    }
}
