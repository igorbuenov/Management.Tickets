namespace Tickets.Application.Interfaces.Messaging
{
    public interface IMessageConsumer
    {
        Task StartAsync(string queueName, CancellationToken cancellationToken);
    }
}
