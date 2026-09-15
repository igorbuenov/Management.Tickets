using Tickets.Domain.Enums;

namespace Tickets.Application.UseCases.Tickets.UpdateStatus
{
    public interface IUpdateTicketStatusUseCase
    {
        Task Execute(int ticketId, TicketStatus status);
    }
}
