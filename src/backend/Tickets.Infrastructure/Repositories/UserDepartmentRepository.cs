using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class UserDepartmentRepository : IUserDepartmentRepository
    {
        private readonly TicketsDbContext _context;

        public UserDepartmentRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserBelongsToDepartment(int userId, int departmentId)
        {
            return await _context.UserDepartments
                .AnyAsync(userDepartment =>
                    userDepartment.UserId == userId &&
                    userDepartment.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<Department>> GetDepartmentsByUserId(int userId)
        {
            return await _context.UserDepartments
                .Where(userDepartment => userDepartment.UserId == userId)
                .Select(userDepartment => userDepartment.Department)
                .OrderBy(department => department.Name)
                .ToListAsync();
        }
    }
}