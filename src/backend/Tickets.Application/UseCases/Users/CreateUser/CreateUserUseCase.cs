using Tickets.Application.DTOs.Users;
using Tickets.Application.Interfaces;
using Tickets.Application.Services.Interfaces;
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

        public CreateUserUseCase(IUserRepository userRepository, IPasswordService passwordService, IPasswordRepository passwordRepository, IUnitOfWork unitOfWork, IUserRoleRepository userRoleRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _passwordRepository = passwordRepository;
            _unitOfWork = unitOfWork;
            _userRoleRepository = userRoleRepository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Execute(CreateUserDto request)
        {

            // Validar request
            await ValidateRequestAsync(request);

            //TODO: Mapear a request em uma entidade de usuário
            var currentUserId = _currentUserService.UserId;

            User user = new User
            {
                Name = request.Name,
                Email = request.Email,
                CreatedByUserId = currentUserId,
            };

            //TODO: Salvar o usuário no banco de dados utilizando o repositório de usuários
            user = await _userRepository.Add(user);

            //TODO: Gerar e Criptografar Senha para o usuário
            string password = _passwordService.GenerateRandomPassword();
            string encryptedPassword = _passwordService.EncryptPassword(password);

            //TODO: Salvar Senhado usuário na tabela de senha
            UserPassword userPassword = new UserPassword
            {
                User = user,
                HashPassword = encryptedPassword,
                ExpirationDate = DateTime.UtcNow, 
                CreatedByUserId = 1
            };

            await _passwordRepository.Add(userPassword);

            await _userRoleRepository.Add(request.RoleID, user);

            await _unitOfWork.Commit();

            return user.Id;

        }

        private async Task ValidateRequestAsync(CreateUserDto request)
        {
            // Implementar validação do request
            var validator = new CreateUserValidator();

            var result = validator.Validate(request);

            var emailExist = await _userRepository.ExistActiveUserWithEmail(request.Email);
            if (emailExist)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "E-mail já está em uso"));
            }

            var role = await _userRoleRepository.GetRoleByID(request.RoleID);
            if (role == null)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("RoleID", "RoleID inválido"));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
