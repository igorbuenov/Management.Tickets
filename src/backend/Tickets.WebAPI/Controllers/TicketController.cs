using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.UseCases.Tickets.CreateTicket;
using Tickets.Application.UseCases.Tickets.GetTickets;
using Tickets.WebAPI.Models.Tickets.Request;
using Tickets.WebAPI.Models.Tickets.Response;

namespace Tickets.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ICreateTicketUseCase _createTicketUseCase;
        private readonly IGetTicketsUseCase _getTicketsUseCase;
        private readonly IMapper _mapper;

        public TicketController(IGetTicketsUseCase getTicketsUseCase, ICreateTicketUseCase createTicketUseCase, IMapper mapper)
        {
            _getTicketsUseCase = getTicketsUseCase;
            _createTicketUseCase = createTicketUseCase;
            _mapper = mapper;
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
            var response = _mapper.Map<GetTicketsResponseModel>(await _getTicketsUseCase.Execute(request.Page, request.PageSize));
            return Ok(response);
        }

    }
}
