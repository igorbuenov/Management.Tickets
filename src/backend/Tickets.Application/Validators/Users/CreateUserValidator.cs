using FluentValidation;
using Tickets.Application.DTOs.Users;

namespace Tickets.Application.Validators.Users
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequestDto> 
    {
        public CreateUserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("O nome do usuário é obrigatório.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("O email do usuário é obrigatório.");

            RuleFor(user => user.RoleID)
                .InclusiveBetween(1, 3)
                .WithMessage("O tipo do usuário deve ser 1 (Admin), 2 (Technician) ou 3 (User).");

            When(user => !string.IsNullOrEmpty(user.Email), () =>
            {
                RuleFor(user => user.Email)
                    .EmailAddress()
                    .WithMessage("O email do usuário deve ser um endereço de email válido.");
            });
        }
    }
}
