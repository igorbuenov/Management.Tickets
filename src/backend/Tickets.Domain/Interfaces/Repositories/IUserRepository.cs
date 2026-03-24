using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> Add(User user);
        Task<bool> ExistActiveUserWithEmail(string email);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetPaged(int page, int pageSize);
        Task<int> Count();
    }
}
