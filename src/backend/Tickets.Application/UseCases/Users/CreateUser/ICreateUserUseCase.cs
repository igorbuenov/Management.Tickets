using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.CreateUser
{
    public interface ICreateUserUseCase
    {
        Task<int> Execute(CreateUserDto request);
    }
}
