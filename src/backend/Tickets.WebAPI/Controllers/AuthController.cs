using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTOs.Auth;
using Tickets.Application.UseCases.Auth;
using Tickets.WebAPI.Models.RequestModels.Auth.Request;

namespace Tickets.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticateUserUseCase _authenticateUserUseCase;

        public AuthController(IMapper mapper, IAuthenticateUserUseCase authenticateUserUseCase)
        {
            _mapper = mapper;
            _authenticateUserUseCase = authenticateUserUseCase;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var response = await _authenticateUserUseCase.Execute(_mapper.Map<LoginRequestDto>(request));

            return Ok(response);
        }
    }
}
