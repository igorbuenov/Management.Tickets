using Tickets.Application.DTOs.Tickets;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Enums;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Tickets.GetTicketMessages
{
    public class GetTicketMessagesUseCase : IGetTicketMessagesUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketMessageRepository _ticketMessageRepository;
        private readonly ICurrentUser _currentUser;
        

        public GetTicketMessagesUseCase(
            ITicketRepository ticketRepository,
            ITicketMessageRepository ticketMessageRepository,
            ICurrentUser currentUser
        )
        {
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<TicketMessageDto>> Execute(int ticketId)
        {
            var ticket = await _ticketRepository.GetById(ticketId);

            if (ticket == null)
                throw new NotFoundException("Ticket não encontrado!");

            if (_currentUser.UserId is null)
                throw new UnauthorizedException("Usuário deve estar autenticado para visualizar as mensagens.");

            var userId = _currentUser.UserId.Value;

            var isTicketParticipant =
                ticket.CreatedByUserId == userId ||
                ticket.AssignedToUserId == userId;

            var isAdmin = _currentUser.Role.Equals(UserRoleEnum.Admin.ToString());

            if (!isTicketParticipant && !isAdmin)
                throw new BusinessRuleException("Você não tem permissão para visualizar as mensagens desse ticket!");

            var messages = await _ticketMessageRepository.GetByTicketId(ticketId);

            return BuildResponse(messages);
        }

        private IEnumerable<TicketMessageDto> BuildResponse(IEnumerable<TicketMessage> ticketMessages)
        {
            return ticketMessages.Select(x => new TicketMessageDto
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.User.Name,
                CreatedAt = x.CreatedAt,
                Message = x.Message,
            }).ToList();
        }
    }
}
