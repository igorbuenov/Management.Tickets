using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface IPasswordResetTokenRepository
    {
        Task Add(PasswordResetToken token);
        Task<PasswordResetToken?> GetByTokenHash(string tokenHash);
    }
}
