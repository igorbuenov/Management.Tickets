using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface IOutboxRepository
    {
        Task Add(OutboxMessage message);
        Task<List<OutboxMessage>> GetPendingMessages(int limit);
    }
}
