namespace Tickets.Application.Interfaces.Messaging
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(string eventType, string content, CancellationToken cancellationToken);
    }
}
