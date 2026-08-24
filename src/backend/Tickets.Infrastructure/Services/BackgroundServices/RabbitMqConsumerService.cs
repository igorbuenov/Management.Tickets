using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tickets.Application.Interfaces.Messaging;

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
                await _consumer.StartAsync(stoppingToken);
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