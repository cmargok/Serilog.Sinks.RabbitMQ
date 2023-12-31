namespace Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exceptions"></param>
    public class EventLogBusException(string message, IEnumerable<Exception> exceptions) : Exception(message: message)
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly IEnumerable<Exception> _innerExceptions = exceptions;
    }
}
