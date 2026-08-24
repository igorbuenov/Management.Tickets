namespace Tickets.Application.Interfaces.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync(string type, string content);
    }
}
