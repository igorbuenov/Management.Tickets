using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Auth.ForgotPassword
{
    public interface IForgotPasswordUseCase
    {
        Task Execute(ForgotPasswordUserRequestDto request);
    }
}
