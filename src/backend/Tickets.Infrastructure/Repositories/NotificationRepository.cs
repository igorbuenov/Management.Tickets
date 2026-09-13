using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly TicketsDbContext _context;

        public NotificationRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task Add(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetByUserId(int userId)
        {
            return await _context.Notifications
                .Where(notification => notification.UserId == userId)
                .OrderByDescending(notification => notification.CreatedAt)
                .ToListAsync();
        }
    }
}
