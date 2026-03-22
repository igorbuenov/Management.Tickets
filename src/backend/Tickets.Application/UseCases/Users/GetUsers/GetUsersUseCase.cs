using Tickets.Application.DTOs.Users;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace Tickets.Application.UseCases.Users.GetUsers
{
    public class GetUsersUseCase : IGetUsersUseCase
    {

        private readonly IUserRepository _userRepository;

        public GetUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUsersResponseDto> Execute()
        {

            var users = await _userRepository.GetAllAsync();

            if (!users.Any())
            {
                //Lançar exception personalizada
            }

            return Response(users);

        }

        private GetUsersResponseDto Response(IEnumerable<User> users)
        {
            return new GetUsersResponseDto
            {
                Success = true,
                Users = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                }).ToList()
            };
        }
    }
}
