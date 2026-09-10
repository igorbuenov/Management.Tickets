using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.UseCases.Tickets.CreateTicket;
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

        public TicketsController(IGetTicketsUseCase getTicketsUseCase, ICreateTicketUseCase createTicketUseCase, IMapper mapper, IGetTicketByIdUseCase getTicketByIdUseCase)
        {
            _getTicketsUseCase = getTicketsUseCase;
            _createTicketUseCase = createTicketUseCase;
            _mapper = mapper;
            _getTicketByIdUseCase = getTicketByIdUseCase;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequestModel request)
        {
            var response =  _mapper.Map<CreateTicketResponseModel>(await _createTicketUseCase.Execute(_mapper.Map<CreateTicketRequestDto>(request)));
            return Ok(response);
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetTickets([FromQuery] GetTicketsRequestModel request)
        {
            var response = _mapper.Map<GetTicketsResponseModel>(await _getTicketsUseCase.Execute(request.Page, request.PageSize, request.Title, request.Priority, request.Status));
            return Ok(response);
        }

        [HttpGet("{id:int}")]
       // [Authorize]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var response = _mapper.Map<TicketModel>(await _getTicketByIdUseCase.Execute(id));
            return Ok(response);
        }

    }
}
