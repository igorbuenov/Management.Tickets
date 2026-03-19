using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        Task Add(int roleID, User user);
        Task<Role> GetRoleByID(int id);
        Task<IEnumerable<Role>> GetRolesByUserId(int userId);
    }
}
