using Tickets.Application.DTOs.Tickets;

namespace Tickets.Application.UseCases.Tickets.GetTicketMessages
{
    public interface IGetTicketMessagesUseCase
    {
        Task<IEnumerable<TicketMessageDto>> Execute(int ticketId);
    }
}