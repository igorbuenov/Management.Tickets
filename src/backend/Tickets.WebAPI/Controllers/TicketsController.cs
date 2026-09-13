using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.UseCases.Tickets.AssignTicket;
using Tickets.Application.UseCases.Tickets.CreateTicket;
using Tickets.Application.UseCases.Tickets.GetAssignedTicket;
using Tickets.Application.UseCases.Tickets.GetCreatedTicketsByUserId;
using Tickets.Application.UseCases.Tickets.GetTicketById;
using Tickets.Application.UseCases.Tickets.GetTickets;
using Tickets.WebAPI.Models.Tickets;
using Tickets.WebAPI.Models.Tickets.Request;
using Tickets.WebAPI.Models.Tickets.Response;

namespace Tickets.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ICreateTicketUseCase _createTicketUseCase;
        private readonly IGetTicketsUseCase _getTicketsUseCase;
        private readonly IGetTicketByIdUseCase _getTicketByIdUseCase; 
        private readonly IMapper _mapper;
        private readonly IAssignTicketUseCase _assignTicketUseCase;
        private readonly IGetAssignedTicketsUseCase _getAssignedTicketsUseCase;
        private readonly IGetCreatedTicketsByUserIdUseCase _getCreatedTicketsByUserIdUseCase;

        public TicketsController(IGetTicketsUseCase getTicketsUseCase, ICreateTicketUseCase createTicketUseCase, IMapper mapper, IGetTicketByIdUseCase getTicketByIdUseCase, IAssignTicketUseCase assignTicketUseCase, IGetAssignedTicketsUseCase getAssignedTicketsUseCase, IGetCreatedTicketsByUserIdUseCase getCreatedTicketsByUserIdUseCase)
        {
            _getTicketsUseCase = getTicketsUseCase;
            _createTicketUseCase = createTicketUseCase;
            _mapper = mapper;
            _getTicketByIdUseCase = getTicketByIdUseCase;
            _assignTicketUseCase = assignTicketUseCase;
            _getAssignedTicketsUseCase = getAssignedTicketsUseCase;
            _getCreatedTicketsByUserIdUseCase = getCreatedTicketsByUserIdUseCase;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequestModel request)
        {
            var response =  _mapper.Map<CreateTicketResponseModel>(await _createTicketUseCase.Execute(_mapper.Map<CreateTicketRequestDto>(request)));
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTickets([FromQuery] GetTicketsRequestModel request)
        {
            var response = _mapper.Map<GetTicketsResponseModel>(await _getTicketsUseCase.Execute(request.Page, request.PageSize, request.Title, request.Priority, request.Status));
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var response = _mapper.Map<TicketModel>(await _getTicketByIdUseCase.Execute(id));
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Technician")]
        [HttpGet("assigned")]
        public async Task<IActionResult> AssignedTo([FromQuery] GetTicketsRequestModel request)
        {
            var response = _mapper.Map<GetTicketsResponseModel>(await _getAssignedTicketsUseCase.Execute(request.Page, request.PageSize, request.Title, request.Priority, request.Status));
            return Ok(response);
        }

        [Authorize]
        [HttpGet("created-by-userid")]
        public async Task<IActionResult> GetTicketsByUserId([FromQuery] GetTicketsRequestModel request)
        {
            var response = _mapper.Map<GetTicketsResponseModel>(await _getCreatedTicketsByUserIdUseCase.Execute(request.Page, request.PageSize, request.Title, request.Priority, request.Status));
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Technician")]
        [HttpPut("{id:int}/assign")]
        public async Task<IActionResult> AssignTicketTo(int id, [FromBody] AssignTicketRequestModel request)
        {
            await _assignTicketUseCase.Execute(id, _mapper.Map<AssignTicketRequestDto>(request));
            return NoContent();
        }

    }
}
