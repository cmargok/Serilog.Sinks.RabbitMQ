using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Sinks.RabbitMQ.Publisher.ClientImplementation;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Entities;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools;

namespace Serilog.Sinks.RabbitMQ.Publisher.Core.Sink.SinkExecutor
{
    public class RabbitMQBatchSink(EventClientConfiguration configuration, ITextFormatter textFormatter) : IBatchedLogEventSink
    {
        private readonly ITextFormatter _formatter = textFormatter;

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            try
            {
                using var client = new RabbitMQClient(configuration);

                foreach (var logEvent in batch)
                {
                    var stringWriter = new StringWriter();

                    _formatter.Format(logEvent, stringWriter);

                    var eventTo = new EventTo
                    {
                        ApiLog = stringWriter.ToString(),
                        ApiLogFrom = configuration.ApiName,
                    };
                    await client.PublishLogAsync(eventTo);
                }

            }
            catch (AggregateException exceptions)
            {
                throw new EventLogBusException("A big problem has occurred", exceptions.InnerExceptions);
            }

        }
        public async Task OnEmptyBatchAsync()
        {
            await Console.Out.WriteLineAsync("");
        }
    }
}
