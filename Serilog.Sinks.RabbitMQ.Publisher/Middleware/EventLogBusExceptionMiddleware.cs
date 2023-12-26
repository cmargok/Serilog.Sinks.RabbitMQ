using Microsoft.AspNetCore.Http;
using Serilog.Sinks.RabbitMQ.TransversalConfiguration.Tools;

namespace EventLogBus.Expose.Middleware
{
    /// <summary>
    /// EventLogBusException Handler middleware Constructor
    /// </summary>
    /// <param name="next"></param>
    public class EventLogBusExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// Invoke next move
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EventLogBusException ex)
            {
                await HandleExceptionAsync(ex);
            }
        }

        public virtual async Task HandleExceptionAsync(EventLogBusException exceptions)
        {
            foreach (var ex in exceptions._innerExceptions)
            {
                await Console.Out.WriteLineAsync("Source ->" + ex.Source + "\n message -> " + ex.Message);
            }

        }

    }
}
