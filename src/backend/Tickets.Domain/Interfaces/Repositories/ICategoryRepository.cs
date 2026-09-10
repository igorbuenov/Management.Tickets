using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> Add(Category department);
        Task<IEnumerable<Category>> GetPaged(int page, int pageSize, string name);
        Task<int> Count(string name);
    }
}
