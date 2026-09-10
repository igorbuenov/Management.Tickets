using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Departments;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Departments.GetDepartments
{
    public class GetDepartmentsUseCase : IGetDepartmentsUseCase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentsUseCase(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<PagedResultDto<DepartmentDto>> Execute(GetDepartmentsRequestDto request)
        {

            if (request.Page <= 0)
                throw new ErrorOnValidationException("Page must be greater than 0");

            if (request.PageSize <= 0)
                throw new ErrorOnValidationException("PageSize must be greater than 0");

            var departments = await _departmentRepository.GetPaged(request.Page, request.PageSize, request.Name);
            var totalDepartments = await _departmentRepository.Count(request.Name);

            return BuildResponse(departments, request.Page, request.PageSize, totalDepartments);
        }

        private PagedResultDto<DepartmentDto> BuildResponse(IEnumerable<Department> departments, int page, int pageSize ,int total)
        {
            return new PagedResultDto<DepartmentDto>
            {
                Items = departments.Select(department => new DepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    CreatedAt = department.CreatedAt,
                    UpdatedAt = department.UpdatedAt
                }).ToList(),

                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }
    }
}
