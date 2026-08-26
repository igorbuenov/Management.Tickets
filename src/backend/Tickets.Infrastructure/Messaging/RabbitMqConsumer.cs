using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Tickets.Application.Events.Users;
using Tickets.Application.Handlers.EventEmailHandler;
using Tickets.Application.Interfaces.Messaging;
using Tickets.Infrastructure.Settings;

namespace Tickets.Infrastructure.Messaging
{
    public class RabbitMqConsumer : IMessageConsumer
    {
        private readonly RabbitMqSettings _settings;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RabbitMqConsumer> _logger;

        public RabbitMqConsumer(
            IOptions<RabbitMqSettings> settings, ILogger<RabbitMqConsumer> logger, IServiceScopeFactory scopeFactory)
        {
            _settings = settings.Value;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(
            string queueName, CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
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


                    object? @event = queueName switch
                    {
                        MessagingQueues.WelcomeEmail
                            => JsonSerializer.Deserialize<CreateUserEmailEvent>(message),

                        MessagingQueues.PasswordRecoveryEmail
                            => JsonSerializer.Deserialize<PasswordRecoveryEmailEvent>(message),

                        _ => throw new InvalidOperationException($"Unknown message type: {queueName}")
                    };

                    if (@event is null)
                    {
                        throw new InvalidOperationException(
                            $"Could not deserialize message from queue {queueName}.");
                    }

                    using var scope = _scopeFactory.CreateScope();

                    var eventEmailHandler = scope.ServiceProvider.GetRequiredService<IEventEmailHandler>();
                    await eventEmailHandler.HandleEventAsync(@event.GetType().Name, @event, cancellationToken);

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