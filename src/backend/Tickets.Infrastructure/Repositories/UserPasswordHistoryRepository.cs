using Microsoft.EntityFrameworkCore;
using Tickets.Application.Commons.Security;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class UserPasswordHistoryRepository : IUserPasswordHistoryRepository
    {
        private readonly TicketsDbContext _context;

        public UserPasswordHistoryRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task Add(UserPasswordHistory userPasswordHistory)
        {
            await _context.UserPasswordHistories.AddAsync(userPasswordHistory);
        }

        public async Task<List<UserPasswordHistory>> GetAllByUserId(int userId)
        {
            return await _context.UserPasswordHistories
                .Where(uph => uph.UserId == userId)
                .ToListAsync();
        }

        public Task<List<UserPasswordHistory>> GetByUserIdForValidateOnChangePassword(int userId)
        {
            return _context.UserPasswordHistories
                .Where(uph => uph.UserId == userId)
                .OrderByDescending(uph => uph.CreatedAt)
                .Take(PasswordPolicy.PasswordHistoryLimit) 
                .ToListAsync();
        }
    }
}
