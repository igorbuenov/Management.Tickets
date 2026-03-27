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


        public UpdateUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser, IValidator<UpdateUserDto> validator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _validator = validator;
        }

        public async Task Execute(UpdateUserDto request)
        {
            var user = await GetValidUser(request);

            if (_currentUser.Role != "Admin")
            {
                if (_currentUser.UserId != request.Id)
                    throw new ErrorOnValidationException("Você não tem permissão para alterar este usuário");
            }

            user.Update(request.Name, request.Email);
            await _unitOfWork.Commit();

        }

        private async Task<User> GetValidUser(UpdateUserDto request)
        {
            ValidateRequest(request);

            var user = await _userRepository.GetById(request.Id);

            if (user == null)
                throw new NotFoundException("Usuário não encontrado");

            if (!user.IsActive)
                throw new ErrorOnValidationException("Usuário inativo não pode ser atualizado");

            if (user.Email != request.Email)
            {
                var emailAlreadyInUse = await _userRepository
                    .ExistActiveUserWithEmail(request.Email);

                if (emailAlreadyInUse)
                    throw new ErrorOnValidationException("Email já está em uso");
            }

            return user;
        }

        private void ValidateRequest(UpdateUserDto request)
        {
            var result = _validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                
                throw new ErrorOnValidationException(errorMessages);
            }
        }

    }
}
