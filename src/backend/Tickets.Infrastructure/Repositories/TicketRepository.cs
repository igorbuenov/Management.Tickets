using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketsDbContext _context;

        public TicketRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task<IEnumerable<Ticket>> GetPaged(int page, int pageSize, string? title, int? priority, int? status)
        {
            var query = _context.Tickets.Include(ticket => ticket.CreatedByUser).Include(ticket => ticket.AssignedToUser).AsQueryable(); 

            if (!string.IsNullOrWhiteSpace(title)) 
            { 
                query = query.Where(ticket => ticket.Title.Contains(title)); 
            }

            if (priority.HasValue) 
            { 
                query = query.Where(ticket => (int)ticket.Priority == priority.Value); 
            }

            if (status.HasValue) 
            { 
                query = query.Where(ticket => (int)ticket.Status == status.Value); 
            }

            return await query.OrderBy(t => t.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> Count(string? title, int? priority, int? status)
        {
            var query = _context.Tickets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title)) 
            { 
                query = query.Where(ticket => ticket.Title.Contains(title)); 
            }

            if (priority.HasValue) 
            { 
                query = query.Where(ticket => (int)ticket.Priority == priority.Value); 
            }

            if (status.HasValue) 
            { 
                query = query.Where(ticket => (int)ticket.Status == status.Value);
            }

            return await query.CountAsync();
        }

        public async Task<Ticket> GetById(int id)
        {
            return await _context.Tickets
                .Include(ticket => ticket.CreatedByUser)
                .Include(ticket => ticket.AssignedToUser)
                .FirstOrDefaultAsync(ticket => ticket.Id == id);
        }
    }
}
