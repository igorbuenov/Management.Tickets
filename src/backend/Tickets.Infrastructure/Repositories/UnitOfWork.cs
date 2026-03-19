using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketsDbContext _context;

        public UnitOfWork(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}

