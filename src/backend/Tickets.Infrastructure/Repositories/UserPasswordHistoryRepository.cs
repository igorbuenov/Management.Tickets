using Microsoft.EntityFrameworkCore;
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
    }
}
