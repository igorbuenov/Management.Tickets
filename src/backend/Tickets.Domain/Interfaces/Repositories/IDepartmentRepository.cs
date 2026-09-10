using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department> Add(Department department);
        Task<IEnumerable<Department>> GetPaged(int page, int pageSize, string name);
        Task<int> Count(string name);
    }
}
