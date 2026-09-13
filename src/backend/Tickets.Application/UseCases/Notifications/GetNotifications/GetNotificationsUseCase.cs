using Tickets.Application.DTOs.Notifications;
using Tickets.Application.Interfaces;
using Tickets.Domain.Interfaces.Repositories;

namespace Tickets.Application.UseCases.Notifications.GetNotifications
{
    public class GetNotificationsUseCase : IGetNotificationsUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUser _currentUser;

        public GetNotificationsUseCase(
            INotificationRepository notificationRepository,
            ICurrentUser currentUser)
        {
            _notificationRepository = notificationRepository;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<NotificationDto>> Execute()
        {
            if (_currentUser.UserId is null)
                throw new UnauthorizedAccessException();

            var notifications =
                await _notificationRepository.GetByUserId(_currentUser.UserId.Value);

            return notifications.Select(notification => new NotificationDto
            {
                Id = notification.Id,
                TicketId = notification.TicketId,
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            });
        }
    }
}
