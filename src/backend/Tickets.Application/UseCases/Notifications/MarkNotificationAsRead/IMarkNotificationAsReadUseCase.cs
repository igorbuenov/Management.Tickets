namespace Tickets.Application.UseCases.Notifications.MarkNotificationAsRead
{
    public interface IMarkNotificationAsReadUseCase
    {
        Task Execute(int notificationId);
    }
}
