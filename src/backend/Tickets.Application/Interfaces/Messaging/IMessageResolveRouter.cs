namespace Tickets.Application.Interfaces.Messaging
{
    public interface IMessageResolveRouter
    {
        string Resolve(string messageType);
    }
}
