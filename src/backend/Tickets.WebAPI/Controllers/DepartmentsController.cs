using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTOs.Departments;
using Tickets.Application.UseCases.Departments.CreateDepartment;
using Tickets.Application.UseCases.Departments.GetDepartments;
using Tickets.Application.UseCases.Departments.GetMyDepartments;
using Tickets.WebAPI.Models.Departments.Request;
using Tickets.WebAPI.Models.Departments.Response;

namespace Tickets.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {

        private readonly ICreateDepartmentUseCase _createDepartmentUseCase;
        private readonly IMapper _mapper;
        private readonly IGetDepartmentsUseCase _getDepartmentsUseCase;
        private readonly IGetMyDepartmentsUseCase _getMyDepartmentsUseCase;

        public DepartmentsController(ICreateDepartmentUseCase createDepartmentUseCase, IMapper mapper, IGetDepartmentsUseCase getDepartmentsUseCase, IGetMyDepartmentsUseCase getMyDepartmentsUseCase)
        {
            _createDepartmentUseCase = createDepartmentUseCase;
            _mapper = mapper;
            _getDepartmentsUseCase = getDepartmentsUseCase;
            _getMyDepartmentsUseCase = getMyDepartmentsUseCase;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequestModel request)
        {
            var response = _mapper.Map<CreateDepartmentResponseModel>(await _createDepartmentUseCase.Execute(_mapper.Map<CreateDepartmentRequestDto>(request)));
            return Created(string.Empty, response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDepartments([FromQuery] GetDerpartmentsRequestModel request)
        {
            var response = _mapper.Map<GetDepartmentsResponseModel>(await _getDepartmentsUseCase.Execute(_mapper.Map<GetDepartmentsRequestDto>(request)));
            return Ok(response);
        }

        [HttpGet("my-departments")]
        [Authorize]
        public async Task<IActionResult> GetMyDepartments()
        {
            var response = await _getMyDepartmentsUseCase.Execute();
            return Ok(response);
        }

    }
}
