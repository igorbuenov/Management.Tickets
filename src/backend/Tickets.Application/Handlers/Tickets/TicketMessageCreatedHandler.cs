using Tickets.Application.Events.Tickets;
using Tickets.Domain.Entities;
using Tickets.Domain.Enums;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.Handlers.Tickets
{
    public class TicketMessageCreatedHandler
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketMessageCreatedHandler(
            ITicketRepository ticketRepository,
            INotificationRepository notificationRepository,
            IUserRoleRepository userRoleRepository,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _notificationRepository = notificationRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(
            TicketMessageCreatedEvent @event,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetById(@event.TicketId);

            if (ticket == null)
                throw new NotFoundException("Ticket não encontrado.");

            var recipients = await GetRecipients(ticket, @event.SenderUserId);

            foreach (var recipientId in recipients)
            {
                var notification = new Notification
                {
                    UserId = recipientId,
                    TicketId = ticket.Id,
                    Message = $"Nova mensagem no ticket #{ticket.Id}",
                    IsRead = false
                };

                await _notificationRepository.Add(notification);
            }

            await _unitOfWork.Commit();
        }

        private async Task<IEnumerable<int>> GetRecipients(
            Ticket ticket,
            int senderUserId)
        {
            var isAdmin = await IsAdmin(senderUserId);

            if (isAdmin)
            {
                return new[]
                {
                    ticket.CreatedByUserId,
                    ticket.AssignedToUserId
                }
                .Where(id => id.HasValue && id.Value != senderUserId)
                .Select(id => id!.Value)
                .Distinct();
            }

            if (senderUserId == ticket.CreatedByUserId)
            {
                return ticket.AssignedToUserId.HasValue
                    ? new[] { ticket.AssignedToUserId.Value }
                    : Enumerable.Empty<int>();
            }

            if (ticket.AssignedToUserId == senderUserId)
            {
                return new[] { ticket.CreatedByUserId };
            }

            return Enumerable.Empty<int>();
        }

        private async Task<bool> IsAdmin(int userId)
        {
            var roles = await _userRoleRepository.GetRolesByUserId(userId);
            return roles.Any(role => role.Id == (int)UserRoleEnum.Admin);
        }
    }
}