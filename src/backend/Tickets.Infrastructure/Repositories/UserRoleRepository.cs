using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly TicketsDbContext _context;

        public UserRoleRepository(TicketsDbContext context)
        {
            _context = context;
        }
        public async Task Add(int roleID, User user)
        {
            await _context.UserRoles.AddAsync(new UserRole 
            { 
                RoleId = roleID, 
                User = user
            });
        }

        public async Task<Role> GetRoleByID(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(role => role.Id == id);
        }

        public async Task<IEnumerable<Role>> GetRolesByUserId(int userId)
        {
            return await _context.UserRoles
                .Where(userRole => userRole.User.Id == userId)
                .Select(userRole => userRole.Role)
                .ToListAsync();
        }
    }
}
