using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.Commons.Security;
using Tickets.Application.DTOs.Auth;

namespace Tickets.Application.Validators.Password
{
    public class ChangeTemporaryPasswordValidator : AbstractValidator<ChangeTemporaryPasswordRequestDto>
    {
        public ChangeTemporaryPasswordValidator()
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

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm password cannot be empty.")
                .Equal(x => x.NewPassword)
                .WithMessage("New password and confirm password do not match.");
        }
    }
}
