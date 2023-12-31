using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Entities;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Settings;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Tools;
using Serilog.Sinks.RabbitMQ.Publisher.RabbitMQHandler.ClientImplementation;

namespace Serilog.Sinks.RabbitMQ.Publisher.Core.Sink.SinkExecutor
{
    internal class RabbitMQBatchSink(EventClientConfiguration configuration, ITextFormatter textFormatter) : IBatchedLogEventSink
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

                    _formatter.Format(logEvent: logEvent, output: stringWriter);

                    var eventTo = new EventTo
                    {
                        ApiLog = stringWriter.ToString(),
                        ApiLogFrom = configuration.ApiName,
                    };
                    await client.PublishLogAsync(@event: eventTo);
                }

            }
            catch (AggregateException exceptions)
            {
                throw new EventLogBusException(message: "A big problem has occurred", exceptions: exceptions.InnerExceptions);
            }

        }
        public async Task OnEmptyBatchAsync()
        {
            await Console.Out.WriteLineAsync("");
        }
    }
}
