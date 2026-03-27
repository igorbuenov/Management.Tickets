using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.UpdateUser
{
    public interface IUpdateUserUseCase
    {
        Task Execute(UpdateUserDto request);
    }
}
