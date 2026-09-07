using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {

        private readonly TicketsDbContext _context;

        public PasswordResetTokenRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task Add(PasswordResetToken passwordResetToken)
        {
            await _context.PasswordResetTokens.AddAsync(passwordResetToken);
        }

        public async Task<PasswordResetToken?> GetByTokenHash(string tokenHash)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
        }
    }
}
