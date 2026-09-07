using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Xml.Linq;
using Tickets.Application.Configurations;
using Tickets.Application.DTOs.Users;
using Tickets.Application.Events.Users;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace Tickets.Application.UseCases.Auth.ForgotPassword
{
    public class ForgotPasswordUseCase : IForgotPasswordUseCase
    {
        

        private readonly IUserRepository _userRepository;
        private readonly ILogger<ForgotPasswordUseCase> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IPasswordResetTokenService _passwordResetTokenService;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly FrontendSettings _frontendSettings;

        public ForgotPasswordUseCase(
            IUserRepository userRepository, 
            ILogger<ForgotPasswordUseCase> logger, 
            IUnitOfWork unitOfWork, 
            IOutboxRepository outboxRepository, 
            IPasswordResetTokenService passwordResetTokenService, 
            IPasswordResetTokenRepository passwordResetTokenRepository, 
            IOptions<FrontendSettings> frontendSettings)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _outboxRepository = outboxRepository;
            _passwordResetTokenService = passwordResetTokenService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _frontendSettings = frontendSettings.Value;
        }

        public async Task Execute(ForgotPasswordUserRequestDto request)
        {
            
            _logger.LogInformation("Executing forgot password use case for email: {Email}", request.Email);
            var user = await _userRepository.GetByEmail(request.Email);
            if (user == null) return;

            var token = _passwordResetTokenService.GenerateToken();
            var tokenHash = _passwordResetTokenService.HashToken(token);

            var passwordResetToken = new PasswordResetToken
            {
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _passwordResetTokenRepository.Add(passwordResetToken);

            var resetLink = $"{_frontendSettings.BaseUrl}/reset-password?token={Uri.EscapeDataString(token)}";

            var passwordRecoveryEvent = new PasswordRecoveryEmailEvent(
                user.Email,
                user.Name,
                resetLink
            );

            var outboxMessage = new OutboxMessage
            {
                Type = nameof(PasswordRecoveryEmailEvent),
                Content = JsonSerializer.Serialize(passwordRecoveryEvent),
                CreatedAt = DateTime.UtcNow
            };

            await _outboxRepository.Add(outboxMessage);
            await _unitOfWork.Commit();

        }
    }
}
