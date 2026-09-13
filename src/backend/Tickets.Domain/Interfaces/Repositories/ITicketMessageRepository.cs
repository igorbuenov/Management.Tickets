using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface ITicketMessageRepository
    {
        Task<TicketMessage> Add(TicketMessage message);
        Task<IEnumerable<TicketMessage>> GetByTicketId(int ticketId);
    }
}
