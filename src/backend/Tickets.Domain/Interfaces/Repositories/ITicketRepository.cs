using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket);
        Task<IEnumerable<Ticket>> GetPaged(int page, int pageSize, string? title, int? priority, int? status); 
        Task<int> Count(string? title, int? priority, int? status);
        Task<Ticket> GetById(int id);
        Task<IEnumerable<Ticket>> GetPagedByAssignedUser(
            int userId,
            int page,
            int pageSize,
            string? title,
            int? priority,
            int? status);

        Task<int> CountByAssignedUser(
            int userId,
            string? title,
            int? priority,
            int? status);

        Task<IEnumerable<Ticket>> GetPagedByCreatedUser(
            int userId,
            int page,
            int pageSize,
            string? title,
            int? priority,
            int? status);

        Task<int> CountByCreatedUser(
            int userId,
            string? title,
            int? priority,
            int? status);

        Task<IEnumerable<Ticket>> GetPagedByDepartments(
            IEnumerable<int> departmentIds,
            int page,
            int pageSize,
            string? title,
            int? priority,
            int? status);

        Task<int> CountByDepartments(
            IEnumerable<int> departmentIds,
            string? title,
            int? priority,
            int? status);
    }
}
