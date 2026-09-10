using Tickets.Application.DTOs.Departments;

namespace Tickets.Application.UseCases.Departments.CreateDepartment
{
    public interface ICreateDepartmentUseCase
    {
        Task<CreateDepartmentResponseDto> Execute(CreateDepartmentRequestDto dto);
    }
}
