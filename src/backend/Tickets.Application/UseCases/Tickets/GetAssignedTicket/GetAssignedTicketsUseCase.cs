using Tickets.Application.DTOs.Categories;
using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Departments;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.DTOs.Users;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Tickets.GetAssignedTicket
{
    public class GetAssignedTicketsUseCase : IGetAssignedTicketsUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICurrentUser _currentUser;

        public GetAssignedTicketsUseCase(ITicketRepository ticketRepository, ICurrentUser currentUser)
        {
            _ticketRepository = ticketRepository;
            _currentUser = currentUser;
        }

        public async Task<PagedResultDto<TicketDto>> Execute(int page = 1, int pageSize = 5, string? title = null, int? priority = null, int? status = null)
        {

            if (page <= 0)
                throw new ErrorOnValidationException("Page must be greater than 0");

            if (pageSize <= 0)
                throw new ErrorOnValidationException("PageSize must be greater than 0");

            var userId = (int)_currentUser.UserId!;

            var ticketsAssignTo = await _ticketRepository.GetPagedByAssignedUser(userId, page, pageSize, title, priority, status);

            var totalTicketsAssignTo = await _ticketRepository.CountByAssignedUser(userId, title, priority, status);

            return BuildResponse(ticketsAssignTo, page, pageSize, totalTicketsAssignTo);
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
