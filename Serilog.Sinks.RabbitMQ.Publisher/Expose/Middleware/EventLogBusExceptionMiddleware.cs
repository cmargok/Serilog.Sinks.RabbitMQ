using Microsoft.AspNetCore.Http;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools;

namespace Serilog.Sinks.RabbitMQ.Publisher.Expose.Middleware
{
    /// <summary>
    /// Middleware to capture and handle exceptions of type EventLogBusException.
    /// </summary>
    /// <param name="next">Delegate for the next function in the middleware chain.</param>
    public class EventLogBusExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// Middleware invocation method.
        /// </summary>
        /// <param name="context">HTTP context of the request.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EventLogBusException ex)
            {       
                // Capture exceptions of type EventLogBusException.
                await HandleExceptionAsync(ex);
            }
        }


        /// <summary>
        /// Handling of EventLogBusException exceptions.
        /// </summary>
        /// <param name="exceptions">Captured EventLogBusException.</param>
        /// <returns>Asynchronous task.</returns>
        public virtual async Task HandleExceptionAsync(EventLogBusException exceptions)
        {
            // Iterate over the inner exceptions and write information to the console.
            foreach (var ex in exceptions._innerExceptions)
            {
                await Console.Out.WriteLineAsync("Source ->" + ex.Source + "\n message -> " + ex.Message);
            }

        }

    }
}
