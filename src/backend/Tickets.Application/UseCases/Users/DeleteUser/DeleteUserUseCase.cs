using Microsoft.Extensions.Logging;
using Tickets.Application.Interfaces;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Users.DeleteUser
{
    public class DeleteUserUseCase : IDeleteUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<DeleteUserUseCase> _logger;

        public DeleteUserUseCase(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            ILogger<DeleteUserUseCase> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task Execute(int id)
        {
            var actor = _currentUser.IsAuthenticated ? _currentUser.UserId.ToString() : "anonymous";
            _logger.LogInformation("Delete user request started for {TargetUserId} by {Actor}", id, actor);

            if (!_currentUser.IsAuthenticated)
            {
                _logger.LogWarning("Unauthorized delete attempt for {TargetUserId} by {Actor}", id, actor);
                throw new UnauthorizedException("User must be authenticated to delete users.");
            }

            if (_currentUser.Role != "Admin")
            {
                _logger.LogWarning("Forbidden delete attempt for {TargetUserId} by {Actor} with role {Role}", id, actor, _currentUser.Role);
                throw new ForbiddenException("Only admins can delete users.");
            }

            var user = await _userRepository.GetById(id);

            if (user is null)
            {
                _logger.LogWarning("Delete failed: user {TargetUserId} not found. Requested by {Actor}", id, actor);
                throw new NotFoundException($"User with id {id} not found.");
            }

            if (user.Id == _currentUser.UserId)
            {
                _logger.LogWarning("Business rule violation: user {Actor} attempted to delete themselves", actor);
                throw new BusinessRuleException("Users cannot delete themselves.");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Business rule violation: user {TargetUserId} is already inactive. Requested by {Actor}", id, actor);
                throw new BusinessRuleException($"User with id {id} is already inactive.");
            }

            user.Deactivate();
            _logger.LogInformation("User {UserId} deactivated by {Actor}", user.Id, actor);

            await _unitOfWork.Commit();
            _logger.LogInformation("Delete user request completed successfully for {UserId}", user.Id);
        }
    }
}
