using Tickets.Domain.Entities;

namespace Tickets.Domain.Interfaces.Repositories
{
    public interface IUserDepartmentRepository
    {        
        Task<bool> UserBelongsToDepartment(int userId, int departmentId);
        Task<IEnumerable<Department>> GetDepartmentsByUserId(int userId);
    }
}
