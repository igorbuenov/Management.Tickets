using Tickets.Application.Commons.Security;
using Tickets.Application.DTOs.Auth;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Auth.ResetPassword
{
    public class ResetPasswordUseCase : IResetPasswordUseCase
    {

        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IPasswordService _passwordService;
        private readonly IPasswordRepository _userPasswordRepository;
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;
        private readonly IPasswordResetTokenService _passwordResetTokenService;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordUseCase(
            IPasswordResetTokenRepository passwordResetTokenRepository,
            IPasswordService passwordService,
            IPasswordRepository userPasswordRepository,
            IUserPasswordHistoryRepository userPasswordHistoryRepository,
            IPasswordResetTokenService passwordResetTokenService,
            IUnitOfWork unitOfWork)
        {
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _passwordService = passwordService;
            _userPasswordRepository = userPasswordRepository;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
            _passwordResetTokenService = passwordResetTokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(ResetPasswordRequestDto request)
        {
            var tokenHash = _passwordResetTokenService.HashToken(request.Token);

            var passwordResetToken =
                await _passwordResetTokenRepository.GetByTokenHash(tokenHash);

            if (passwordResetToken == null)
                throw new BusinessRuleException("Invalid or expired password reset token.");

            if (passwordResetToken.UsedAt.HasValue)
                throw new BusinessRuleException("Invalid or expired password reset token.");

            if (passwordResetToken.ExpiresAt <= DateTime.UtcNow)
                throw new BusinessRuleException("Invalid or expired password reset token.");

            var userPassword = await _userPasswordRepository.GetByUserId(passwordResetToken.UserId);

            if (userPassword == null)
                throw new BusinessRuleException("User not found.");

            var passwordHash = _passwordService.HashPassword(request.NewPassword);

            userPassword.Update(
                passwordHash,
                DateTime.UtcNow.AddDays(PasswordPolicy.ExpirationDays),
                passwordResetToken.UserId);

            var passwordHistory = new UserPasswordHistory
            {
                UserId = passwordResetToken.UserId,
                HashPassword = passwordHash,
                CreatedAt = DateTime.UtcNow,
            };

            await _userPasswordHistoryRepository.Add(passwordHistory);

            passwordResetToken.UsedAt = DateTime.UtcNow;

            await _unitOfWork.Commit();

        }
    }
}
