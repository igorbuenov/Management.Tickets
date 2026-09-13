using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class TicketMessageRepository : ITicketMessageRepository
    {
        private readonly TicketsDbContext _context;

        public TicketMessageRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task Add(TicketMessage message)
        {
            await _context.TicketMessages.AddAsync(message);
        }

        public async Task<IEnumerable<TicketMessage>> GetByTicketId(int ticketId)
        {
            return await _context.TicketMessages
                .Include(message => message.User)
                .Where(message => message.TicketId == ticketId)
                .OrderBy(message => message.CreatedAt)
                .ToListAsync();
        }
    }
}
