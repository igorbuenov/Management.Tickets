using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task Add(Notification notification);
        Task<IEnumerable<Notification>> GetByUserId(int userId);
        Task<Notification?> GetById(int notificationId, int userId);
    }
}
