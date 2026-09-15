using System.Text.Json;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.Events.Tickets;
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
        private readonly IOutboxRepository _outboxRepository;

        public CreateTicketMessageUseCase(ITicketMessageRepository ticketMessageRepository, ICurrentUser currentUser, ITicketRepository ticketRepository, IUnitOfWork unitOfWork, IOutboxRepository outboxRepository)
        {
            _ticketMessageRepository = ticketMessageRepository;
            _currentUser = currentUser;
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _outboxRepository = outboxRepository;
        }

        public async Task Execute(int ticketId, CreateTicketMessageRequestDto request)
        {
            var ticket = await _ticketRepository.GetById(ticketId);
            if (ticket == null)
                throw new NotFoundException("Ticket não encontrado!");

            if(ticket.Status == TicketStatus.Resolved || ticket.Status == TicketStatus.Closed)
                throw new BusinessRuleException("Não é possivel enviar mensagens para ticket resolvido ou fechado!");

            if (_currentUser.UserId is null)
                throw new UnauthorizedException("Usuário deve estar autenticado para enviar mensagem.");

            var userId = _currentUser.UserId.Value;

            var isTicketParticipant =
                ticket.CreatedByUserId == userId ||
                ticket.AssignedToUserId == userId;

            var isAdmin = _currentUser.Role.Equals(UserRoleEnum.Admin.ToString());

            if (!isTicketParticipant && !isAdmin)
            {
                throw new BusinessRuleException("Você não tem permissão para responder esse ticket!");
            }

            var ticketMessage = new TicketMessage
            {
                TicketId = ticketId,
                UserId = userId,
                Message = request.Message,
            };

            ticketMessage = await _ticketMessageRepository.Add(ticketMessage);

            var @event = new TicketMessageCreatedEvent
            {
                TicketId = ticketMessage.TicketId,
                SenderUserId = ticketMessage.UserId
            };

            var outboxMessage = new OutboxMessage
            {
                Type = nameof(TicketMessageCreatedEvent),
                Content = JsonSerializer.Serialize(@event)
            };

            await _outboxRepository.Add(outboxMessage);

            await _unitOfWork.Commit();

        }
    }
}
