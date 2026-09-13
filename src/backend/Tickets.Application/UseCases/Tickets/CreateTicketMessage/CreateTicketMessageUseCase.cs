using Tickets.Application.DTOs.Tickets;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Enums;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Tickets.CreateTicketMessage
{
    public class CreateTicketMessageUseCase : ICreateTicketMessageUseCase
    {
        private readonly ITicketMessageRepository _ticketMessageRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicketMessageUseCase(ITicketMessageRepository ticketMessageRepository, ICurrentUser currentUser, ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
        {
            _ticketMessageRepository = ticketMessageRepository;
            _currentUser = currentUser;
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(int ticketId, CreateTicketMessageRequestDto request)
        {
            var ticket = await _ticketRepository.GetById(ticketId);
            if (ticket == null)
                throw new NotFoundException("Ticket não encontrado!");

            if (_currentUser.UserId is null)
                throw new UnauthorizedException("Usuário deve estar autenticado para enviar mensagem.");

            var userId = _currentUser.UserId.Value;

            var isTicketParticipant =
                ticket.CreatedByUserId == userId ||
                ticket.AssignedToUserId == userId;

            var isAdmin = _currentUser.Role.Equals(UserRoleEnum.Admin.ToString());

            if (!isTicketParticipant && !isAdmin)
            {
                throw new BusinessRuleException(
                    "Você não tem permissão para responder esse ticket!");
            }

            var ticketMessage = new TicketMessage
            {
                TicketId = ticketId,
                UserId = userId,
                Message = request.Message,
            };

            await _ticketMessageRepository.Add(ticketMessage);
            await _unitOfWork.Commit();

        }
    }
}
