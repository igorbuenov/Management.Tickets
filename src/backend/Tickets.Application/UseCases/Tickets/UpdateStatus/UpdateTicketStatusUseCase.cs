using Tickets.Application.Interfaces;
using Tickets.Domain.Enums;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Tickets.UpdateStatus
{
    public class UpdateTicketStatusUseCase : IUpdateTicketStatusUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTicketStatusUseCase(
            ITicketRepository ticketRepository,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(int ticketId, TicketStatus status)
        {
            var ticket = await _ticketRepository.GetById(ticketId);

            if (ticket == null)
                throw new NotFoundException("Ticket não encontrado.");

            if (ticket.CreatedByUserId == _currentUser.UserId)
                throw new BusinessRuleException("Usuários nãopodem alterar o status do próprio ticket.");

            bool isAdmin = _currentUser.Role.Equals(UserRoleEnum.Admin.ToString());
            bool isTechnician = _currentUser.Role.Equals(UserRoleEnum.Technician.ToString());

            if (!isAdmin && !isTechnician)
                throw new BusinessRuleException("Apenas administradores e técnicos podem alterar o status do ticket.");

            ticket.Status = status;
            ticket.UpdatedAt = DateTime.Now;

            await _unitOfWork.Commit();
        }
    }
}
