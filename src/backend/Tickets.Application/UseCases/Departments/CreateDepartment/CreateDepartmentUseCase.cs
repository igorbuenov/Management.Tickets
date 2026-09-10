using Tickets.Application.DTOs.Departments;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Departments.CreateDepartment
{
    public class CreateDepartmentUseCase : ICreateDepartmentUseCase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDepartmentUseCase(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateDepartmentResponseDto> Execute(CreateDepartmentRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
                throw new ErrorOnValidationException("O nome deve ser preenchido");

            Department department = new Department
            {
                Name = dto.Name,
            };

            department = await _departmentRepository.Add(department);
            await _unitOfWork.Commit();

            return BuildResponse(department);
        }

        private CreateDepartmentResponseDto BuildResponse(Department department)
        {
            return new CreateDepartmentResponseDto
            {
                Success = true,
                Department = new DepartmentDto
                {
                    Id = department.Id,
                    CreatedAt = department.CreatedAt,
                    UpdatedAt = department.UpdatedAt,
                    Name = department.Name
                }
            };
        }
    }
}
