using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Exceptions;
using Tickets.Application.Events.Users;
using Tickets.Application.Interfaces.Messaging;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Messaging;

namespace Tickets.Infrastructure.Services.BackgroundServices
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxProcessor> _logger;

        public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var outboxRepository = 
                    scope.ServiceProvider
                    .GetRequiredService<IOutboxRepository>();

                var publisher = 
                    scope.ServiceProvider
                    .GetRequiredService<IMessagePublisher>();

                var unitOfWork = 
                    scope.ServiceProvider
                    .GetRequiredService<IUnitOfWork>();

                var messages =
                    await outboxRepository.GetPendingMessages(10);

                foreach (var message in messages)
                {
                    try
                    {
                        var queueName = message.Type switch
                        {
                            nameof(CreateUserEmailEvent)
                            => MessagingQueues.WelcomeEmail,

                            nameof(PasswordRecoveryEmailEvent)
                            => MessagingQueues.PasswordRecoveryEmail,

                            _=> throw new InvalidOperationException($"Unknown message type: {message.Type}")
                        };

                        await publisher.PublishAsync(message.Type, message.Content, queueName);

                        message.ProcessedAt = DateTime.Now;

                        _logger.LogInformation($"Outbox message {message.Id} published succefully.");
                    }
                    catch (OperationInterruptedException ex)
                    {
                        _logger.LogError(
                            ex,
                            $"Operation interrupted while processing outbox message {message.Id}. Attempt: {message.RetryCount}",
                            message.Id,
                            message.RetryCount);

                        message.RetryCount++;
                        message.Error = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        message.RetryCount++;
                        message.Error = ex.Message;

                        _logger.LogError(
                            ex, 
                            $"Error processing outbox message {message.Id}. Attempt: {message.RetryCount}",
                            message.Id,
                            message.RetryCount);
                    }
                }


                await unitOfWork.Commit();

                await Task.Delay(
                    TimeSpan.FromSeconds(5),
                    stoppingToken);

            }
        }
    }
}
