using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket);
        Task<IEnumerable<Ticket>> GetPaged(int page, int pageSize);
        Task<int> Count();
    }
}
