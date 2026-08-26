using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tickets.Application.Interfaces.Messaging;
using Tickets.Infrastructure.Messaging;

namespace Tickets.Infrastructure.Services.BackgroundServices
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly IMessageConsumer _consumer;
        private readonly ILogger<RabbitMqConsumerService> _logger;

        public RabbitMqConsumerService(
            IMessageConsumer consumer,
            ILogger<RabbitMqConsumerService> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "RabbitMQ Consumer Service started.");

            try
            {
                var consumers = new[]
                {
                     _consumer.StartAsync( 
                         MessagingQueues.WelcomeEmail, 
                         stoppingToken),

                     _consumer.StartAsync(
                         MessagingQueues.PasswordRecoveryEmail,
                         stoppingToken),
                };

                await Task.WhenAll(consumers);

            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "RabbitMQ Consumer Service stopping.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in RabbitMQ Consumer Service.");
            }
        }
    }
}