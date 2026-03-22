using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.GetUsers
{
    public interface IGetUsersUseCase
    {
        Task<GetUsersResponseDto> Execute();
    }
}
