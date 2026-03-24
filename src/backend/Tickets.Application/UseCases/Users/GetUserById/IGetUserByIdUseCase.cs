using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.GetUserById
{
    public interface IGetUserByIdUseCase
    {
        Task<UserDto> Execute(int id);
    }
}
