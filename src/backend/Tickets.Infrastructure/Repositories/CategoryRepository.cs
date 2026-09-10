using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;

namespace Tickets.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TicketsDbContext _context;

        public CategoryRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task<Category> Add(Category category)
        {
            await _context.Categories.AddAsync(category);
            return category;
        }

        public async Task<IEnumerable<Category>> GetPaged(int page, int pageSize, string name)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(category => category.Name.Contains(name));
            }

            return await query
                .OrderBy(category => category.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }

        public async Task<int> Count(string name)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(category => category.Name.Contains(name));
            }

            return await query.CountAsync();
            
        }

        
    }
}
