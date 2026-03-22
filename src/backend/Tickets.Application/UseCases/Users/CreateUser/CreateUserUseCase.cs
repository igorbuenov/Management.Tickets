using Microsoft.Extensions.Logging;
using Tickets.Application.DTOs.Users;
using Tickets.Application.Interfaces;
using Tickets.Application.Services.Interfaces;
using Tickets.Application.Validators.Users;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Users.CreateUser
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordRepository _passwordRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CreateUserUseCase> _logger;

        public CreateUserUseCase(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IPasswordRepository passwordRepository,
            IUnitOfWork unitOfWork,
            IUserRoleRepository userRoleRepository,
            ICurrentUserService currentUserService,
            ILogger<CreateUserUseCase> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _passwordRepository = passwordRepository;
            _unitOfWork = unitOfWork;
            _userRoleRepository = userRoleRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<CreateUserResponseDto> Execute(CreateUserDto request)
        {
            _logger.LogInformation("Create user request started for {Email} with Role {RoleId}", request.Email, request.RoleID);

            // Validar request
            await ValidateRequestAsync(request);

            var currentUserId = _currentUserService.UserId;

            User user = new User
            {
                Name = request.Name,
                Email = request.Email,
                CreatedByUserId = currentUserId,
            };

            user = await _userRepository.Add(user);
            _logger.LogInformation("User entity persisted with Id {UserId} by {CreatedBy}", user.Id, currentUserId);

            // Gerar e Criptografar Senha para o usuário
            string password = _passwordService.GenerateRandomPassword();
            string encryptedPassword = _passwordService.EncryptPassword(password);

            // Salvar Senhado usuário na tabela de senha
            UserPassword userPassword = new UserPassword
            {
                User = user,
                HashPassword = encryptedPassword,
                ExpirationDate = DateTime.UtcNow,
                CreatedByUserId = currentUserId
            };

            await _passwordRepository.Add(userPassword);
            _logger.LogInformation("Password record created for user {UserId} (expiration: {Expiration})", user.Id, userPassword.ExpirationDate);

            await _userRoleRepository.Add(request.RoleID, user);
            _logger.LogInformation("Role {RoleId} assigned to user {UserId}", request.RoleID, user.Id);

            await _unitOfWork.Commit();
            _logger.LogInformation("Create user request completed successfully for {UserId}", user.Id);

            // TODO: Implementar serviço de email - Enviar Email com a senha para o usuário

            return Response(user, request.RoleID);
        }

        private async Task ValidateRequestAsync(CreateUserDto request)
        {
            _logger.LogInformation("Validating create user request for {Email}", request.Email);

            var validator = new CreateUserValidator();

            var result = validator.Validate(request);

            var emailExist = await _userRepository.ExistActiveUserWithEmail(request.Email);
            if (emailExist)
            {
                _logger.LogWarning("Validation failed for {Email}: email already in use", request.Email);
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "E-mail já está em uso"));
            }

            var role = await _userRoleRepository.GetRoleByID(request.RoleID);
            if (role == null)
            {
                _logger.LogWarning("Validation failed for {Email}: invalid RoleID {RoleID}", request.Email, request.RoleID);
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("RoleID", "RoleID inválido"));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Create user validation failed for {Email}: {Errors}", request.Email, string.Join("; ", errorMessages));

                throw new ErrorOnValidationException(errorMessages);
            }

            _logger.LogInformation("Create user request validation passed for {Email}", request.Email);
        }

        private CreateUserResponseDto Response(User user, int roleID)
        {
            return new CreateUserResponseDto
            {
                Success = true,
                User = new CreateUserDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    RoleID = roleID
                }
            };
        }
    }
}
