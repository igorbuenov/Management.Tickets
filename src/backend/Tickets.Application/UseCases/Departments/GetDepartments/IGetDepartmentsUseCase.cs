using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Departments;

namespace Tickets.Application.UseCases.Departments.GetDepartments
{
    public interface IGetDepartmentsUseCase
    {
        Task<PagedResultDto<DepartmentDto>> Execute(GetDepartmentsRequestDto dto);
    }
}
