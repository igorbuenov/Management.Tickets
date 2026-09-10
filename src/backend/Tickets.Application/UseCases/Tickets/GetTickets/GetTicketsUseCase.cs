using System.Net.Sockets;
using Tickets.Application.DTOs.Categories;
using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Departments;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.DTOs.Users;
using Tickets.Application.UseCases.Tickets.GetTickets;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Tickets
{
    public class GetTicketsUseCase : IGetTicketsUseCase
    {

        private readonly ITicketRepository _ticketRepository;

        public GetTicketsUseCase(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<PagedResultDto<TicketDto>> Execute(int page = 1, int pageSize = 5, string? title = null, int? priority = null, int? status = null)
        {

            if (page <= 0)
                throw new ErrorOnValidationException("Page must be greater than 0");

            if (pageSize <= 0)
                throw new ErrorOnValidationException("PageSize must be greater than 0");

            var tickets = await _ticketRepository.GetPaged(page, pageSize, title, priority, status);
            
            var totalTickets = await _ticketRepository.Count(title, priority, status);

            return BuildResponse(tickets, page, pageSize, totalTickets);
        }

        private PagedResultDto<TicketDto> BuildResponse(IEnumerable<Ticket> tickets, int page, int pageSize, int total)
        {
            return new PagedResultDto<TicketDto>
            {
                Items = tickets.Select(t => new TicketDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority.ToString(),
                    Status = t.Status.ToString(),
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    Category = new CategoryDto
                    {
                        Id = t.CategoryId,
                        Name = t.Category.Name,
                    },
                    Department = new DepartmentDto
                    {
                        Id = t.DepartmentId,
                        Name = t.Department.Name,
                    },
                    CreatedBy = new UserSummaryDto
                    {
                        Id = t.CreatedByUser.Id,
                        Name = t.CreatedByUser.Name
                    },
                    AssignedTo = t.AssignedToUser == null
                    ? null
                    : new UserSummaryDto
                    {
                        Id = t.AssignedToUser.Id,
                        Name = t.AssignedToUser.Name
                    }
                }).ToList(),

                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }
    }
}
