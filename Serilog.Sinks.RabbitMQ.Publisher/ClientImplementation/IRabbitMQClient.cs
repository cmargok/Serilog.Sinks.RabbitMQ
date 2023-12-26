using Serilog.Sinks.RabbitMQ.Publisher.Configuration.Entities;

namespace Serilog.Sinks.RabbitMQ.Publisher.ClientImplementation
{
    public interface IRabbitMQClient : IDisposable
    {
        new void Dispose();
        Task PublishLogAsync(EventTo @event);
    }
}