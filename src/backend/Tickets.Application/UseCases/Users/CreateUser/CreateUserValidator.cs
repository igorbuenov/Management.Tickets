using FluentValidation;
using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto> 
    {
        public CreateUserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("O nome do usuário é obrigatório.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("O email do usuário é obrigatório.")
                .EmailAddress()
                .WithMessage("O email do usuário deve ser um endereço de email válido.");

            RuleFor(user => user.RoleID)
                .NotEmpty()
                .WithMessage("O tipo do usuário é obrigatório.");

        }


    }
}
