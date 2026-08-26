namespace Tickets.Application.Handlers.EventEmailHandler
{
    public interface IEventEmailHandler
    {
        Task HandleEventAsync(string type, object content, CancellationToken cancellationToken);
    }
}
