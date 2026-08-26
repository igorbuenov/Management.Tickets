using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Xml.Linq;
using Tickets.Application.DTOs.Users;
using Tickets.Application.Events.Users;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace Tickets.Application.UseCases.Users.ForgotPassword
{
    public class ForgotPasswordUseCase : IForgotPasswordUseCase
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IPasswordRepository _passwordRepository;
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;
        private readonly ILogger<ForgotPasswordUseCase> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOutboxRepository _outboxRepository;

        public ForgotPasswordUseCase(IUserRepository userRepository, IPasswordService passwordService, IPasswordRepository passwordRepository, IUserPasswordHistoryRepository userPasswordHistoryRepository, IUnitOfWork unitOfWork, ILogger<ForgotPasswordUseCase> logger, IOutboxRepository outboxRepository)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _passwordRepository = passwordRepository;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _outboxRepository = outboxRepository;
        }

        public async Task Execute(ForgotPasswordUserRequestDto request)
        {
            _logger.LogInformation("Executing forgot password use case for email: {Email}", request.Email);
            var user = await _userRepository.GetByEmail(request.Email);
            if (user == null)
                throw new Exception("Se o email estiver registrado, você receberá instruções para redefinir sua senha.");

            string temporaryPassword = _passwordService.GenerateRandomPassword();
            string hashPassword = _passwordService.HashPassword(temporaryPassword);

            var currentPassword = await _passwordRepository.GetByUserId(user.Id);

            currentPassword.Update(
                hashPassword,
                DateTime.UtcNow,
                user.Id);

            _logger.LogInformation("Password record updated for user {UserId} (expiration: {Expiration})", user.Id, currentPassword.ExpirationDate);
            UserPasswordHistory passwordHistory = new UserPasswordHistory
            {
                UserId = user.Id,
                HashPassword = hashPassword,
                CreatedAt = DateTime.UtcNow
            };

            var passwordRecoveryEvent = new PasswordRecoveryEmailEvent(
                user.Email,
                user.Name,
                temporaryPassword
            );

            var outboxMessage = new OutboxMessage
            {
                Type = nameof(PasswordRecoveryEmailEvent),
                Content = JsonSerializer.Serialize(passwordRecoveryEvent),
                CreatedAt = DateTime.UtcNow
            };

            await _outboxRepository.Add(outboxMessage);

            _logger.LogInformation("Adding password history record for user {UserId}", user.Id);
            await _userPasswordHistoryRepository.Add(passwordHistory);

            await _unitOfWork.Commit();

        }
    }
}
