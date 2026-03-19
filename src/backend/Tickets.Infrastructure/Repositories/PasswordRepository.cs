using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class PasswordRepository : IPasswordRepository
    {
        private readonly TicketsDbContext _context;

        public PasswordRepository(TicketsDbContext context)
        {
            _context = context;
        }
        public async Task Add(UserPassword userPassword)
        {
            await _context.UserPasswords.AddAsync(userPassword);
        }

        public async Task<UserPassword> GetByUserId(int userId)
        {
            return await _context.UserPasswords.FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
