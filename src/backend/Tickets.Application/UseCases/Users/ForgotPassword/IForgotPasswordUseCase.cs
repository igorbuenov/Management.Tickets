using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.ForgotPassword
{
    public interface IForgotPasswordUseCase
    {
        Task Execute(ForgotPasswordUserRequestDto request);
    }
}
