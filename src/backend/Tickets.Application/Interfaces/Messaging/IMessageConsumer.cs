namespace Tickets.Application.Interfaces.Messaging
{
    public interface IMessageConsumer
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}
