using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog.Sinks.RabbitMQ.TransversalConfiguration;
using Serilog.Sinks.RabbitMQ.TransversalConfiguration.Entities;
using System.Text;
namespace Serilog.Sinks.RabbitMQ.ClientImplementation
{
    public class RabbitMQClient : IRabbitMQClient
    {
        // synchronization locks
        private const int MaxChannelCount = 64;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        private readonly SemaphoreSlim[] _modelLocks = new SemaphoreSlim[MaxChannelCount];
        private int _currentModelIndex = -1;

        // cancellation token
        private readonly CancellationTokenSource _closeTokenSource = new();
        private readonly CancellationToken _closeToken;

        // configuration member
        private readonly EventClientConfiguration _config;
        private readonly PublicationAddress _publicationAddress;

        // endpoint members
        private readonly ConnectionFactory _connectionFactory;
        private readonly IModel[] _models = new IModel[MaxChannelCount];
        private readonly IBasicProperties[] _properties = new IBasicProperties[MaxChannelCount];
        private volatile IConnection? _connection;

        public RabbitMQClient(EventClientConfiguration configuration)
        {
            _closeToken = _closeTokenSource.Token;

            for (var i = 0; i < MaxChannelCount; i++)
            {
                _modelLocks[i] = new SemaphoreSlim(1, 1);
            }

            // load configuration
            _config = configuration;

            _publicationAddress = new PublicationAddress(_config.ExchangeType, _config.ExchangeName, _config.RouteKey);

            // initialize
            _connectionFactory = SetConnectionFactory(configuration);
        }

        private static ConnectionFactory SetConnectionFactory(EventClientConfiguration configuration)
            => new()
            {
                UserName = configuration.Username,
                Password = configuration.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
            };

        public async Task PublishAsync(EventTo @event)
        {
            var currentModelIndex = Interlocked.Increment(ref _currentModelIndex);

            currentModelIndex = (currentModelIndex % MaxChannelCount + MaxChannelCount) % MaxChannelCount;

            var modelLock = _modelLocks[currentModelIndex];

            await modelLock.WaitAsync(_closeToken);
            try
            {
                var channel = _models[currentModelIndex];

                var properties = _properties[currentModelIndex];

                if (channel == null)
                {
                    var connection = await GetConnectionAsync();

                    channel = connection.CreateModel();

                    _models[currentModelIndex] = channel;

                    properties = channel.CreateBasicProperties();

                    properties.DeliveryMode = (byte)_config.DeliveryMode; // persistence

                    _properties[currentModelIndex] = properties;
                }

                var message = JsonConvert.SerializeObject(@event);

                var body = Encoding.UTF8.GetBytes(message);
                // push message to exchange
                channel.BasicPublish(_publicationAddress, properties, body);
            }
            finally
            {
                modelLock.Release();
            }
        }
        private async Task<IConnection> GetConnectionAsync()
        {
            if (_connection == null)
            {
                await _connectionLock.WaitAsync(_closeToken);
                try
                {
                    _connection ??= _config.Hostnames.Count == 0
                        ? _connectionFactory.CreateConnection()
                        : _connectionFactory.CreateConnection(_config.Hostnames);
                }
                finally
                {
                    _connectionLock.Release();
                }
            }
            return _connection;
        }
        public void Dispose()
        {
            Close();

            _closeTokenSource.Dispose();

            _connectionLock.Dispose();

            foreach (var modelLock in _modelLocks)
                modelLock.Dispose();

            foreach (var model in _models)
                model?.Dispose();

            _connection?.Dispose();

            GC.SuppressFinalize(this);
        }

        private void Close()
        {
            List<Exception> exceptions = [];
            try
            {
                _closeTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            for (var i = 0; i < _models.Length; i++)
            {
                try
                {
                    _modelLocks[i].Wait(10);
                    _models[i]?.Close();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            try
            {
                _connectionLock.Wait(10);
                _connection?.Close();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }

    }
}
