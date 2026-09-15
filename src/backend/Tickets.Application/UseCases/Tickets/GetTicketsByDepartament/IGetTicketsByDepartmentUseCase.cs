using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Tickets;

namespace Tickets.Application.UseCases.Tickets.GetTicketsByDepartament
{
    public interface IGetTicketsByDepartmentUseCase
    {
        Task<PagedResultDto<TicketDto>> Execute(
            int page = 1,
            int pageSize = 5,
            string? title = null,
            int? priority = null,
            int? status = null);
    }
}