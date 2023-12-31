using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Entities;

namespace Serilog.Sinks.RabbitMQ.Publisher.RabbitMQHandler.Contract
{
    internal interface IRabbitMQClient : IDisposable
    {
        public new void Dispose();
        public Task PublishLogAsync(EventTo @event);
    }
}