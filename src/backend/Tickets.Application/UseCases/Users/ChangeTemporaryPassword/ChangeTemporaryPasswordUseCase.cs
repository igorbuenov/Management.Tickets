using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.Commons.Security;
using Tickets.Application.DTOs.Users;
using Tickets.Application.Interfaces;
using Tickets.Application.UseCases.Users.ChangePassword;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Users.ChangeTemporaryPassword
{
    public class ChangeTemporaryPasswordUseCase : IChangeTemporaryPasswordUseCase
    {
        private readonly ICurrentUser _currentUser;
        private readonly IPasswordService _passwordService;
        private readonly IPasswordRepository _passwordRepository;
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;
        private readonly IValidator<ChangeTemporaryPasswordRequestDto> _validator;
        private readonly ILogger<ChangeTemporaryPasswordRequestDto> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeTemporaryPasswordUseCase(ICurrentUser currentUser, IPasswordService passwordService, IPasswordRepository passwordRepository, IUserPasswordHistoryRepository userPasswordHistoryRepository, IValidator<ChangeTemporaryPasswordRequestDto> validator, ILogger<ChangeTemporaryPasswordRequestDto> logger, IUnitOfWork unitOfWork)
        {
            _currentUser = currentUser;
            _passwordService = passwordService;
            _passwordRepository = passwordRepository;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        public async Task Execute(ChangeTemporaryPasswordRequestDto request, int id)
        {
            if (id != _currentUser.UserId)
            {
                throw new ForbiddenException("You can only change your own password.");
            }

            ValidateRequest(request);

            var currentPassword = await _passwordRepository.GetByUserId(id);
            if (currentPassword is null)
                throw new NotFoundException("Current password record not found for the user.");

            if (currentPassword.ExpirationDate >= DateTime.Now)
                throw new BusinessRuleException("Password change is not required.");

            await ValidatePasswordHistory(request, id);

            var newHashPassword = _passwordService.HashPassword(request.NewPassword);

            currentPassword.Update(
                newHashPassword,
                DateTime.Now.AddDays(PasswordPolicy.ExpirationDays),
                id);

            UserPasswordHistory passwordHistory = new UserPasswordHistory
            {
                UserId = id,
                HashPassword = newHashPassword,
                CreatedAt = DateTime.Now
            };

            _logger.LogInformation("Adding password history record for user {UserId}", id);
            await _userPasswordHistoryRepository.Add(passwordHistory);

            await _unitOfWork.Commit();
        }
        private void ValidateRequest(ChangeTemporaryPasswordRequestDto request)
        {
            var result = _validator.Validate(request);
            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(x => x.ErrorMessage).ToList());
            }
        }

        private async Task ValidatePasswordHistory(ChangeTemporaryPasswordRequestDto request, int userId)
        {

            List<UserPasswordHistory> userPasswordHistories = await _userPasswordHistoryRepository.GetByUserIdForValidateOnChangePassword(userId);

            if (userPasswordHistories.Any(uph => _passwordService.VerifyPassword(request.NewPassword, uph.HashPassword)))
            {
                throw new ErrorOnValidationException($"New password cannot be the same as any of the last {PasswordPolicy.PasswordHistoryLimit} passwords.");
            }
        }
    }
}
