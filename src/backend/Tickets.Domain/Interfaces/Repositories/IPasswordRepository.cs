using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface IPasswordRepository
    {
        Task Add(UserPassword userPassword);
        Task<UserPassword> GetByUserId(int userId);
    }
}
