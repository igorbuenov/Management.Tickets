using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket);
        Task<IEnumerable<Ticket>> GetPaged(int page, int pageSize, string? title, int? priority, int? status); 
        Task<int> Count(string? title, int? priority, int? status);
        Task<Ticket> GetById(int id);
    }
}
