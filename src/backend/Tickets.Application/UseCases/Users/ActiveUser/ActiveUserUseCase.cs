using Microsoft.Extensions.Logging;
using Tickets.Application.Interfaces;
using Tickets.Application.UseCases.Users.DeleteUser;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Users.ActiveUser
{
    public class ActiveUserUseCase : IActiveUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<ActiveUserUseCase> _logger;

        public ActiveUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser, ILogger<ActiveUserUseCase> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task Execute(int id)
        {
            var actor = _currentUser.IsAuthenticated ? _currentUser.UserId.ToString() : "anonymous";
            _logger.LogInformation("Active user request started for {TargetUserId} by {Actor}", id, actor);

            if (!_currentUser.IsAuthenticated)
            {
                _logger.LogWarning("Unauthorized activate attempt for {TargetUserId} by {Actor}", id, actor);
                throw new UnauthorizedException("User must be authenticated to activate users.");
            }

            if (_currentUser.Role != "Admin")
            {
                _logger.LogWarning("Forbidden activate attempt for {TargetUserId} by {Actor} with role {Role}", id, actor, _currentUser.Role);
                throw new ForbiddenException("Only admins can activate users.");
            }

            var user = await _userRepository.GetById(id);

            if (user is null)
            {
                _logger.LogWarning("Activate failed: user {TargetUserId} not found. Requested by {Actor}", id, actor);
                throw new NotFoundException($"User with id {id} not found.");
            }

            if (user.Id == _currentUser.UserId)
            {
                _logger.LogWarning("Business rule violation: user {Actor} attempted to activate themselves", actor);
                throw new BusinessRuleException("Users cannot activate themselves.");
            }

            if (user.IsActive)
            {
                _logger.LogWarning("Business rule violation: user {Actor} attempted to activate themselves", actor);
                throw new BusinessRuleException($"User with id {id} is already active.");
            }

            user.Activate();
            _logger.LogInformation("User {UserId} activated by {Actor}", user.Id, actor);

            await _unitOfWork.Commit();
            _logger.LogInformation("Activate user request completed successfully for {UserId}", user.Id);
        }
    }
}
