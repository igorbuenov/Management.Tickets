using Tickets.Application.DTOs.Auth;

namespace Tickets.Application.UseCases.Auth
{
    public interface IAuthenticateUserUseCase
    {
        Task<LoginResponseDto> Execute(LoginRequestDto request);
    }
}
