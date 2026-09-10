using Tickets.Application.DTOs.Departments;

namespace Tickets.Application.UseCases.Departments.GetMyDepartments
{
    public interface IGetMyDepartmentsUseCase
    {
        Task<IEnumerable<DepartmentDto>> Execute();
    }
}
