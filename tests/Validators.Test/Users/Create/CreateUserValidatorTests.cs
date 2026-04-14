using CommonTestUtilities.Requests;
using FluentAssertions;
using Tickets.Application.Validators.Users;

namespace Validators.Tests.Users.Create
{
    public class CreateUserValidatorTests
    {

        [Fact]
        public void Success_When_Request_Is_Correct()
        {
            // Arrange
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();

        }

        [Fact]
        public void Error_When_Name_Is_Empty()
        {
            // Arrange
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            request.Name = string.Empty;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals("O nome do usuário é obrigatório."));

        }

        [Fact]
        public void Error_When_Email_Is_Empty()
        {
            // Arrange
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            request.Email = string.Empty;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals("O email do usuário é obrigatório."));
        }

        [Fact]
        public void Error_When_Email_Is_Invalid()
        {
            // Arrange
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            request.Email = "email.com";

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals("O email do usuário deve ser um endereço de email válido."));
        }

        [Fact]
        public void Error_When_Role_Is_Valid()
        {
            // Arrange
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            
            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
            
        }

        [Fact]
        public void Error_When_Role_Is_Invalid()
        {
            // Arrange
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            request.RoleID = 4;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals("O tipo do usuário deve ser 1 (Admin), 2 (Technician) ou 3 (User)."));

        }
    }
}
