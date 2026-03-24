using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.GetUsers
{
    public interface IGetUsersUseCase
    {
        Task<PagedResultDto<UserDto>> Execute(int page, int pageSize);
    }
}
