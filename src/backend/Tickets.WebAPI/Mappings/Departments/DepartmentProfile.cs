using AutoMapper;
using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Departments;
using Tickets.WebAPI.Models.Departments;
using Tickets.WebAPI.Models.Departments.Request;
using Tickets.WebAPI.Models.Departments.Response;

namespace Tickets.WebAPI.Mappings.Departments
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentDto, DepartmentModel>();
            CreateMap<CreateDepartmentRequestModel, CreateDepartmentRequestDto>();
            CreateMap<CreateDepartmentResponseDto, CreateDepartmentResponseModel>();
            CreateMap<GetDerpartmentsRequestModel, GetDepartmentsRequestDto>();
            CreateMap<PagedResultDto<DepartmentDto>, GetDepartmentsResponseModel>();
        }
    }
}
