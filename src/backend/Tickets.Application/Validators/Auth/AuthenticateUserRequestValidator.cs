using FluentValidation;
using Tickets.Application.DTOs.Auth;

namespace Tickets.Application.Validators.Auth
{
    public class AuthenticateUserRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public AuthenticateUserRequestValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("O email do usuário é obrigatório.")
                .EmailAddress()
                .WithMessage("O email do usuário deve ser um endereço de email válido.");

            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("A senha do usuário é obrigatória.");

            When(user => !string.IsNullOrEmpty(user.Email), () =>
            {
                RuleFor(user => user.Email)
               .EmailAddress()
               .WithMessage("O email do usuário é obrigatório.");
            });
        }
    }
}
