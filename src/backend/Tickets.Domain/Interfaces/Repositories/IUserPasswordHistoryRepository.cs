using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface IUserPasswordHistoryRepository
    {
        Task Add(UserPasswordHistory userPasswordHistory);
        Task<List<UserPasswordHistory>> GetAllByUserId(int userId);
        Task<List<UserPasswordHistory>> GetByUserIdForValidateOnChangePassword(int userId);
    }
}
