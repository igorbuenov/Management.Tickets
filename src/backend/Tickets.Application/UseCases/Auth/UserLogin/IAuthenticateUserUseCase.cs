using Tickets.Application.DTOs.Auth;

namespace Tickets.Application.UseCases.Auth.UserLogin
{
    public interface IAuthenticateUserUseCase
    {
        Task<LoginResponseDto> Execute(LoginRequestDto request);
    }
}
