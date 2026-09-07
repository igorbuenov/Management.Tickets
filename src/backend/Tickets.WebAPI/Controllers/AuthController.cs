using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTOs.Auth;
using Tickets.Application.DTOs.Users;
using Tickets.Application.UseCases.Auth.ForgotPassword;
using Tickets.Application.UseCases.Auth.ResetPassword;
using Tickets.Application.UseCases.Auth.UserLogin;
using Tickets.WebAPI.Models.Auth.Request;
using Tickets.WebAPI.Models.Auth.Response;
using Tickets.WebAPI.Models.Users.Request;

namespace Tickets.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticateUserUseCase _authenticateUserUseCase;
        private readonly IForgotPasswordUseCase _forgotPasswordUseCase;
        private readonly IResetPasswordUseCase _resetPasswordUseCase;

        public AuthController(IMapper mapper, IAuthenticateUserUseCase authenticateUserUseCase, IForgotPasswordUseCase forgotPasswordUseCase, IResetPasswordUseCase resetPasswordUseCase)
        {
            _mapper = mapper;
            _authenticateUserUseCase = authenticateUserUseCase;
            _forgotPasswordUseCase = forgotPasswordUseCase;
            _resetPasswordUseCase = resetPasswordUseCase;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var responseDto = await _authenticateUserUseCase.Execute(_mapper.Map<LoginRequestDto>(request));
            var responseModel = _mapper.Map<LoginResponseModel>(responseDto);

            return Ok(responseModel);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordUserRequestModel request)
        {
            await _forgotPasswordUseCase.Execute(_mapper.Map<ForgotPasswordUserRequestDto>(request));
            return Ok("Se o email estiver registrado, você receberá instruções para redefinir sua senha.");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel request)
        {
            await _resetPasswordUseCase.Execute(_mapper.Map<ResetPasswordRequestDto>(request));
            return NoContent();
        }


    }
}
