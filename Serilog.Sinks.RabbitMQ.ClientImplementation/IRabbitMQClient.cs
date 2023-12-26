using Serilog.Sinks.RabbitMQ.TransversalConfiguration.Entities;

namespace Serilog.Sinks.RabbitMQ.ClientImplementation
{
    public interface IRabbitMQClient : IDisposable
    {
        new void Dispose();
        Task PublishLogAsync(EventTo @event);
    }
}