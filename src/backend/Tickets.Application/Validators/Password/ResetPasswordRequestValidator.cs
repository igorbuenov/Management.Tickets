using FluentValidation;
using Tickets.Application.Commons.Security;
using Tickets.Application.DTOs.Auth;

namespace Tickets.Application.Validators.Password
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestDto>
    {

        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New password cannot be empty.")
                .Length(PasswordPolicy.MinLength, PasswordPolicy.MaxLength)
                .WithMessage($"New password must contain between {PasswordPolicy.MinLength} and {PasswordPolicy.MaxLength} characters.")
                .Matches(PasswordPolicy.Regex)
                .WithMessage(PasswordPolicy.DefaultValidationMessage)
                .Must(password => !password.Any(char.IsWhiteSpace))
                .WithMessage("New password cannot contain spaces.");
        }

        
    }
}
