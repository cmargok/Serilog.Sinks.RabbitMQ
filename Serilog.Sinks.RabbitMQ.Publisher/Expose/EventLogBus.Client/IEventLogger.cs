namespace Serilog.Sinks.RabbitMQ.Publisher.Expose.EventLogBus.Client
{
    /// <summary>
    /// Represents an interface for event logging functionality.
    /// </summary>
    public interface IEventLogger
    {
        /// <summary>
        /// Logs informational messages.
        /// </summary>
         /// <param name="message">The informational message to be logged.</param>
        public void LoggingInformation(string message);

        /// <summary>
        /// Logs warning messages.
        /// </summary>
        /// <param name="message">The warning message to be logged.</param>
        public void LoggingWarning(string message);

        /// <summary>
        /// Logs error messages along with the associated exception details.
        /// </summary>
        /// <param name="ex">The exception object to be logged.</param>
        /// <param name="message">The error message to be logged.</param>
        public void LoggingError(Exception ex, string message);
    }
}
