using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Tickets.Application.Interfaces.Messaging;
using Tickets.Infrastructure.Settings;

namespace Tickets.Infrastructure.Messaging
{
    public class RabbitMqConsumer : IMessageConsumer
    {
        private readonly RabbitMqSettings _settings;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly IEventDispatcher _eventDispatcher;

        public RabbitMqConsumer(
            IOptions<RabbitMqSettings> settings, ILogger<RabbitMqConsumer> logger, IEventDispatcher eventDispatcher)
        {
            _settings = settings.Value;
            _logger = logger;
            _eventDispatcher = eventDispatcher;
        }

        public async Task StartAsync(
            string queueName, CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,

                Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = _settings.Host
                }
            };

            await using var connection =
                await factory.CreateConnectionAsync(cancellationToken);

            await using var channel =
                await connection.CreateChannelAsync(
                    cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var eventType = args.BasicProperties?.Type;
                    

                    await _eventDispatcher.DispatchAsync(
                        eventType,
                        message,
                        cancellationToken);

                    await channel.BasicAckAsync(
                        args.DeliveryTag,
                        multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error processing RabbitMQ message.");

                    await channel.BasicNackAsync(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: true);
                }
            };

            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken);

            await Task.Delay(
                Timeout.Infinite,
                cancellationToken);
        }
    }
}