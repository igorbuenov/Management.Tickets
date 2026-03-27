using FluentValidation;
using Tickets.Application.DTOs.Users;

namespace Tickets.Application.Validators.Users
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {

        public UpdateUserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("O nome do usuário é obrigatório.");

            RuleFor(user => user.Email)
                    .NotEmpty()
                    .WithMessage("O email do usuário é obrigatório.")
                    .EmailAddress()
                    .WithMessage("O email do usuário deve ser um endereço de email válido.");
        }
        
    }
}
