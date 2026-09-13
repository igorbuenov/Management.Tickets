using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Tickets;

namespace Tickets.Application.UseCases.Tickets.GetAssignedTicket
{
    public interface IGetAssignedTicketsUseCase
    {
        Task<PagedResultDto<TicketDto>> Execute(int page, int pageSize, string? title, int? priority, int? status);
    }
}
