using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.UseCases.Tickets.GetTickets;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace Tickets.Application.UseCases.Tickets
{
    public class GetTicketsUseCase : IGetTicketsUseCase
    {

        private readonly ITicketRepository _ticketRepository;

        public GetTicketsUseCase(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<PagedResultDto<TicketDto>> Execute(int page = 1, int pageSize = 5)
        {

            if (page <= 0)
                throw new ArgumentException("Page must be greater than 0");

            if (pageSize <= 0)
                throw new ArgumentException("PageSize must be greater than 0");

            var tickets = await _ticketRepository.GetPaged(page, pageSize);
            
            var totalTickets = await _ticketRepository.Count();

            return BuildResponse(tickets, page, pageSize, totalTickets);
        }

        private PagedResultDto<TicketDto> BuildResponse(IEnumerable<Ticket> tickets, int page, int pageSize, int total)
        {
            return new PagedResultDto<TicketDto>
            {
                Items = tickets.Select(t => new TicketDto
                {
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority.ToString(),
                    Status = t.Status.ToString(),
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    CreatedByUserId = t.CreatedByUserId,
                    AssignedToUserId = t.AssignedToUserId
                }).ToList(),

                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }
    }
}
