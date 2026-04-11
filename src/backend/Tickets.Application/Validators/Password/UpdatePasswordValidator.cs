using FluentValidation;
using Tickets.Application.Commons.Security;
using Tickets.Application.DTOs.Users;

namespace Tickets.Application.Validators.Password
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordRequestDto>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password cannot be empty.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New password cannot be empty.")
                .Length(PasswordPolicy.MinLength, PasswordPolicy.MaxLength)
                .WithMessage($"New password must contain between {PasswordPolicy.MinLength} and {PasswordPolicy.MaxLength} characters.")
                .Matches(PasswordPolicy.Regex)
                .WithMessage(PasswordPolicy.DefaultValidationMessage)
                .Must(password => !password.Any(char.IsWhiteSpace))
                .WithMessage("New password cannot contain spaces.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm password cannot be empty.")
                .Equal(x => x.NewPassword)
                .WithMessage("New password and confirm password do not match.");

            RuleFor(x => x)
                .Must(x => x.CurrentPassword != x.NewPassword)
                .WithMessage("New password must be different from current password.");
        }
    }
}
