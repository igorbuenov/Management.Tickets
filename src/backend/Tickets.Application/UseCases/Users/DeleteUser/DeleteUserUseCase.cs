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
        public DeleteUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task Execute(int id)
        {
            if(!_currentUser.IsAuthenticated) 
                throw new UnauthorizedException("User must be authenticated to delete users.");

            if (_currentUser.Role != "Admin") 
                throw new ForbiddenException("Only admins can delete users.");
            
            var user = await _userRepository.GetById(id);

            if (user is null)
                throw new NotFoundException($"User with id {id} not found.");

            if (user.Id == _currentUser.UserId)
                throw new BusinessRuleException("Users cannot delete themselves.");

            if (!user.IsActive)
                throw new BusinessRuleException($"User with id {id} is already inactive.");

            user.Deactivate();

            await _unitOfWork.Commit();

        }
    }
}
