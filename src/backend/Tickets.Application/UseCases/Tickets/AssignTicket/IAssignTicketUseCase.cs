using Tickets.Application.DTOs.Tickets;

namespace Tickets.Application.UseCases.Tickets.AssignTicket
{
    public interface IAssignTicketUseCase
    {
        Task Execute(int ticketId, AssignTicketRequestDto request);
    }
}
