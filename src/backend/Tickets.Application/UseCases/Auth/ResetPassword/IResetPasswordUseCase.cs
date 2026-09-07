using Tickets.Application.DTOs.Auth;

namespace Tickets.Application.UseCases.Auth.ResetPassword
{
    public interface IResetPasswordUseCase
    {
        Task Execute(ResetPasswordRequestDto request);
    }
}
