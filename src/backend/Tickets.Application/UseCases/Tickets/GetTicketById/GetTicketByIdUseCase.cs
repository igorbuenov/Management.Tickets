
using Microsoft.Extensions.Logging;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.DTOs.Users;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Tickets.GetTicketById
{
    public class GetTicketByIdUseCase : IGetTicketByIdUseCase
    {

        private readonly ITicketRepository _ticketRepository;
        private readonly ILogger<GetTicketByIdUseCase> _logger;

        public GetTicketByIdUseCase(ITicketRepository ticketRepository, ILogger<GetTicketByIdUseCase> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
        }

        public async Task<TicketDto> Execute(int id)
        {
            _logger.LogInformation("Get ticket by id request started for {TicketId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Validation failed for GetTicketById: invalid ticket ID {UserId}", id);
                throw new ErrorOnValidationException("Invalid ticket ID");
            }

            var ticket = await _ticketRepository.GetById(id);
            if (ticket is null)
            {
                _logger.LogWarning("Ticket with ID {TicketId} not found.",id);
                throw new NotFoundException($"Ticket with ID {id} not found.");
            }

            _logger.LogInformation("Ticket with ID {TicketId} retrieved successfully.",id);
            return BuildResponse(ticket);
            
        }

        private TicketDto BuildResponse(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                Title = ticket.Title,
                Description = ticket.Description,
                Priority = ticket.Priority.ToString(),
                Status = ticket.Status.ToString(),

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
            };
        }
    }
}
