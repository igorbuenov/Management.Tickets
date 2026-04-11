using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Users;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Users.GetUsers
{
    public class GetUsersUseCase : IGetUsersUseCase
    {

        private readonly IUserRepository _userRepository;

        public GetUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<UserDto>> Execute(int page, int pageSize)
        {
            if (page <= 0)
                throw new ErrorOnValidationException("Page must be greater than 0");

            if (pageSize <= 0)
                throw new ErrorOnValidationException("PageSize must be greater than 0");

            var users = await _userRepository.GetPaged(page, pageSize);
            if (users == null || !users.Any())
                throw new NotFoundException("No users found for the given page and page size");

            var total = await _userRepository.Count();

            return BuildResponse(users, page, pageSize, total);
        }

        private PagedResultDto<UserDto> BuildResponse(IEnumerable<User> users, int page, int pageSize, int total)
        {

            return new PagedResultDto<UserDto>
            {
                Items = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    IsActive = u.IsActive
                }).ToList(),

                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }
    }
}
