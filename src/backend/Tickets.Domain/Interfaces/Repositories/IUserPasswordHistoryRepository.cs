using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface IUserPasswordHistoryRepository
    {
        Task Add(UserPasswordHistory userPasswordHistory);
    }
}
