using FluentValidation;
using Tickets.Application.DTOs.Users;
using Tickets.Application.Interfaces;
using Tickets.Application.Validators.Users;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Users.UpdateUser
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateUserDto> _validator;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<UpdateUserUseCase> _logger;

        public UpdateUserUseCase(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IValidator<UpdateUserDto> validator,
            ILogger<UpdateUserUseCase> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _validator = validator;
            _logger = logger;
        }

        public async Task Execute(UpdateUserDto request)
        {
            _logger.LogInformation("Update user request started for {TargetUserId} by {CurrentUserId}", request.Id, _currentUser.UserId);

            var user = await GetValidUser(request);

            if (_currentUser.Role != "Admin")
            {
                if (_currentUser.UserId != request.Id)
                {
                    _logger.LogWarning("User {CurrentUserId} attempted to update user {TargetUserId} without permission", _currentUser.UserId, request.Id);
                    throw new ErrorOnValidationException("Você não tem permissão para alterar este usuário");
                }
            }

            user.Update(request.Name, request.Email);

            await _unitOfWork.Commit();

            _logger.LogInformation("User {TargetUserId} updated successfully by {CurrentUserId}", user.Id, _currentUser.UserId);
        }

        private async Task<User> GetValidUser(UpdateUserDto request)
        {
            _logger.LogInformation("Validating update request for user {TargetUserId}", request.Id);

            ValidateRequest(request);

            var user = await _userRepository.GetById(request.Id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {TargetUserId} not found", request.Id);
                throw new NotFoundException("Usuário não encontrado");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Attempt to update inactive user {TargetUserId}", request.Id);
                throw new ErrorOnValidationException("Usuário inativo não pode ser atualizado");
            }

            if (user.Email != request.Email)
            {
                var emailAlreadyInUse = await _userRepository
                    .ExistActiveUserWithEmail(request.Email);

                if (emailAlreadyInUse)
                {
                    _logger.LogWarning("Email {Email} already in use when updating user {TargetUserId}", request.Email, request.Id);
                    throw new ErrorOnValidationException("Email já está em uso");
                }
            }

            return user;
        }

        private void ValidateRequest(UpdateUserDto request)
        {
            var result = _validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                _logger.LogWarning("UpdateUser validation failed for {TargetUserId}: {Errors}", request.Id, string.Join("; ", errorMessages));
                
                throw new ErrorOnValidationException(errorMessages);
            }
        }

    }
}
