using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.ChangePassword
{
    public interface IUpdatePasswordUseCase
    {
        Task Execute(UpdatePasswordRequestDto request, int userId);
    }
}
