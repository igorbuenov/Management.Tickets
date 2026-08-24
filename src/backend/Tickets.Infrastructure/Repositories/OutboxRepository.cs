using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {

        private readonly TicketsDbContext _context;

        public OutboxRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task Add(OutboxMessage message)
        {
            await _context.OutboxMessages.AddAsync(message);
        }

        public async Task<List<OutboxMessage>> GetPendingMessages(int limit)
        {
            return await _context.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .OrderBy(m => m.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }
    }
}
