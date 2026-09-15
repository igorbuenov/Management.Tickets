using Tickets.Application.Interfaces;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Notifications.MarkNotificationAsRead
{
    public class MarkNotificationAsReadUseCase : IMarkNotificationAsReadUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotificationAsReadUseCase(
            INotificationRepository notificationRepository,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(int notificationId)
        {
            if (_currentUser.UserId is null)
                throw new UnauthorizedException("Usuário não autenticado");

            var userId = _currentUser.UserId.Value;

            var notification = await _notificationRepository.GetById(notificationId, userId);
            if (notification is null)
                throw new NotFoundException("Notificação não encontrada.");

            notification.IsRead = true;

            await _unitOfWork.Commit();
        }
    }
}
