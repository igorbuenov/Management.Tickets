using Microsoft.Extensions.Logging;
using Tickets.Application.DTOs.Users;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Users.GetUserById
{
    public class GetUserByIdUseCase : IGetUserByIdUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUserByIdUseCase> _logger;

        public GetUserByIdUseCase(IUserRepository userRepository, ILogger<GetUserByIdUseCase> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserDto> Execute(int id)
        {
            _logger.LogInformation("Get user by id request started for {UserId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Validation failed for GetUserById: invalid user ID {UserId}", id);
                throw new ErrorOnValidationException("Invalid user ID");
            }

            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new Exception($"User with ID {id} not found.");
            }

            _logger.LogInformation("User with ID {UserId} retrieved successfully.", id);

            return BuildResponse(user);
        }

        private UserDto BuildResponse(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsActive = user.IsActive
            };
        }
    }
}
