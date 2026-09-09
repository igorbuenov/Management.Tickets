using Tickets.Application.DTOs.Tickets;
using Tickets.Application.DTOs.Users;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Enums;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Tickets.CreateTicket
{
    public class CreateTicketUseCase : ICreateTicketUseCase
    {

        private readonly ITicketRepository _ticketRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicketUseCase(ITicketRepository ticketRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTicketResponseDto> Execute(CreateTicketRequestDto dto)
        {
            Ticket ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = TicketStatus.Open,
                CreatedAt = DateTime.Now
            };
            
            var userId = _currentUser.UserId;
            if (userId == null)
                throw new UnauthorizedException("User must be authenticated to create tickets.");

            ticket.CreatedByUserId = (int) userId!;

            await _ticketRepository.AddAsync(ticket);
            await _unitOfWork.Commit();

            var createdTicket = await _ticketRepository.GetById(ticket.Id);

            return BuildResponse(createdTicket);
        }

        public CreateTicketResponseDto BuildResponse(Ticket ticket)
        {
            return new CreateTicketResponseDto
            {
                Success = true,
                Ticket = new TicketDto
                {
                    Title = ticket.Title,
                    Description = ticket.Description,
                    Priority = ticket.Priority.ToString(),
                    Status = ticket.Status.ToString(),
                    CreatedAt = ticket.CreatedAt,
                    UpdatedAt = ticket.UpdatedAt,
                    CreatedBy = new UserSummaryDto
                    {
                        Id = ticket.CreatedByUser.Id,
                        Name = ticket.CreatedByUser.Name
                    },
                    AssignedTo = ticket.AssignedToUser == null
                    ? null
                    : new UserSummaryDto
                    {
                        Id = ticket.AssignedToUser.Id,
                        Name = ticket.AssignedToUser.Name
                    }
                }
            };
        }
    }
}
