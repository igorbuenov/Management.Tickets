using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.DeleteUser
{
    public interface IDeleteUserUseCase
    {
        Task Execute(int id);
    }
}
