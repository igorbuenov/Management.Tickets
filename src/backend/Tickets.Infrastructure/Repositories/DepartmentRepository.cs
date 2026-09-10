using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly TicketsDbContext _context;

        public DepartmentRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task<Department> Add(Department department)
        {
            await _context.Departments.AddAsync(department);
            return department;
        }

        public async Task<IEnumerable<Department>> GetPaged(int page, int pageSize, string name)
        {
            var query = _context.Departments.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(department => department.Name.Contains(name));
            }

            return await query
                .OrderBy(department => department.Name)
                .Skip((page - 1) * pageSize )
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> Count(string name)
        {
            var query = _context.Departments.AsQueryable();
            
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }

            return await query.CountAsync();
        }

        
    }
}
