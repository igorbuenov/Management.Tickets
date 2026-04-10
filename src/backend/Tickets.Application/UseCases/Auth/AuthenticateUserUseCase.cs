using Microsoft.Extensions.Logging;
using Tickets.Application.DTOs.Auth;
using Tickets.Application.Interfaces;
using Tickets.Application.Validators.Auth;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Auth
{
    public class AuthenticateUserUseCase : IAuthenticateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordRepository _passwordRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthenticateUserUseCase> _logger;

        public AuthenticateUserUseCase(
            IUserRepository userRepository,
            IPasswordRepository passwordRepository,
            IUserRoleRepository userRoleRepository,
            IPasswordService passwordService,
            IJwtService jwtService,
            ILogger<AuthenticateUserUseCase> logger)
        {
            _userRepository = userRepository;
            _passwordRepository = passwordRepository;
            _userRoleRepository = userRoleRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<LoginResponseDto> Execute(LoginRequestDto request)
        {
            _logger.LogInformation("Login attempt for {Email}", request.Email);

            ValidateRequest(request);

            var user = await ValidateCredentials(request);

            var roles = await _userRoleRepository.GetRolesByUserId(user.Id);

            var token = _jwtService.GenerateToken(user.Id, user.Email, roles);

            _logger.LogInformation(
                "User {UserId} authenticated successfully with roles {Roles}",
                user.Id,
                string.Join(",", roles));

            return Response(token, user, roles);
        }

        private void ValidateRequest(LoginRequestDto request)
        {
            var validator = new AuthenticateUserRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                _logger.LogWarning("Login validation failed: Email or Password not provided");
                throw new AuthValidationException(errorMessages);
            }
        }

        private async Task<User> ValidateCredentials(LoginRequestDto request)
        {

            _logger.LogInformation("Validating credentials user request for {Email}", request.Email);

            var user = await _userRepository.GetByEmail(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed for {Email}: User not found", request.Email);

                /*
                    Validação fake para igualar o tempo de resposta,
                    evitando ataques de timing attack para descobrir se um email existe ou não no sistema.
                */
                _passwordService.VerifyPassword(request.Password, "fake_hash_to_equalize_timing");

                throw new AuthValidationException("Invalid Credentials");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed for {Email}: User is inactive", request.Email);
                throw new AuthValidationException("Invalid Credentials");
            }

            var password = await _passwordRepository.GetByUserId(user.Id);

            bool validPassword = _passwordService.VerifyPassword(request.Password, password.HashPassword);

            if (!validPassword)
            {
                _logger.LogWarning("Login failed for {Email}: Invalid password", request.Email);
                throw new AuthValidationException("Invalid Credentials");
            }

            _logger.LogInformation("Credentials validated for user {UserId}", user.Id);

            return user;
        }

        private LoginResponseDto Response(JsonTokenResultDto token, User user, IEnumerable<Role> roles)
        {
            return new LoginResponseDto
            {
                AccessToken = token.AccessToken,
                ExpiresAt = token.ExpiresAt,
                User = new LoginUserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Roles = roles.Select(r => r.Description).ToList()
                }
            };
        }
    }
}