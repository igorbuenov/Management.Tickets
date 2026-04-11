
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tickets.Application.Commons.Security;
using Tickets.Application.DTOs.Users;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Users.ChangePassword
{
    public class UpdatePasswordUseCase : IUpdatePasswordUseCase
    {
        private readonly ICurrentUser _currentUser;
        private readonly IPasswordService _passwordService;
        private readonly IPasswordRepository _passwordRepository;
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;
        private readonly IValidator<UpdatePasswordRequestDto> _validator;
        private readonly ILogger<UpdatePasswordUseCase> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePasswordUseCase(ICurrentUser currentUser, IPasswordService passwordService, IPasswordRepository passwordRepository, IUserPasswordHistoryRepository userPasswordHistoryRepository, IValidator<UpdatePasswordRequestDto> validator, ILogger<UpdatePasswordUseCase> logger, IUnitOfWork unitOfWork)
        {
            _currentUser = currentUser;
            _passwordService = passwordService;
            _passwordRepository = passwordRepository;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(UpdatePasswordRequestDto request, int userId)
        {
            if (userId != _currentUser.UserId)
            {
                throw new BusinessRuleException("You can only change your own password.");
            }

            ValidateRequest(request);

            var currentPassword = await _passwordRepository.GetByUserId(userId);
            if (currentPassword is null)
            {
                throw new NotFoundException("Current password record not found for the user.");
            }

            if (!_passwordService.VerifyPassword(request.CurrentPassword, currentPassword.HashPassword))
            {
                throw new ErrorOnValidationException("Incorrect password.");
            }

            await ValidatePasswordHistory(request, userId);

            var newHashPassword = _passwordService.HashPassword(request.NewPassword);

            currentPassword.Update(
                newHashPassword, 
                DateTime.UtcNow.AddDays(PasswordPolicy.ExpirationDays), 
                userId);

            _logger.LogInformation("Password record updated for user {UserId} (expiration: {Expiration})", userId, currentPassword.ExpirationDate);

            UserPasswordHistory passwordHistory = new UserPasswordHistory
            {
                UserId = userId,
                HashPassword = newHashPassword,
                CreatedAt = DateTime.UtcNow
            };

            _logger.LogInformation("Adding password history record for user {UserId}", userId);
            await _userPasswordHistoryRepository.Add(passwordHistory);

            await _unitOfWork.Commit();

        }

        private void ValidateRequest(UpdatePasswordRequestDto request)
        {
            var result = _validator.Validate(request);
            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(x => x.ErrorMessage).ToList());
            }
        }

        private async Task ValidatePasswordHistory(UpdatePasswordRequestDto request, int userId)
        {

            List<UserPasswordHistory> userPasswordHistories = await _userPasswordHistoryRepository.GetByUserIdForValidateOnChangePassword(userId);

            if (userPasswordHistories.Any(uph => _passwordService.VerifyPassword(request.NewPassword, uph.HashPassword)))
            {
                throw new ErrorOnValidationException($"New password cannot be the same as any of the last {PasswordPolicy.PasswordHistoryLimit} passwords.");
            }
        }
    }
}
