using Tickets.Application.DTOs.Auth;
using Tickets.Application.Services.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Auth
{
    // Ensure the class implements the interface
    public class AuthenticateUserUseCase : IAuthenticateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordRepository _passwordRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthenticateUserUseCase(
            IUserRepository userRepository,
            IPasswordRepository passwordRepository,
            IUserRoleRepository userRoleRepository,
            IPasswordService passwordService,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordRepository = passwordRepository;
            _userRoleRepository = userRoleRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> Execute(LoginRequestDto request)
        {
            var user = await ValidateRequest(request);

            var roles = await _userRoleRepository.GetRolesByUserId(user.Id);

            var token = _jwtService.GenerateToken(user.Id ,user.Email, roles);

            return new LoginResponseDto
            {
                AccessToken = token,
                ExpiresIn = 3600
            };

        }

        private async Task<User> ValidateRequest(LoginRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                throw new AuthValidationException("Email and Password are required");

            var user = await _userRepository.GetByEmail(request.Email);
            if (user == null)
            {
                /*
                    Validação fake para igualar o tempo de resposta,
                    evitando ataques de timing attack para descobrir se um email existe ou não no sistema.
                 */
                _passwordService.VerifyPassword(request.Password, "fake_hash_to_equalize_timing");
                throw new AuthValidationException("Invalid Credentials");
            }

            if (!user.IsActive)
                throw new AuthValidationException("Invalid Credentials");

            var password = await _passwordRepository.GetByUserId(user.Id);
            bool validPassword = _passwordService.VerifyPassword(request.Password, password.HashPassword);
            if (!validPassword)
                throw new AuthValidationException("Invalid Credentials");

            

            return user;
        }

    }
}
