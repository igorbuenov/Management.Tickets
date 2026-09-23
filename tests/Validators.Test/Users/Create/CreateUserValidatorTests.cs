using CommonTestUtilities.Requests;
using FluentAssertions;
using FluentValidation.TestHelper;
using Tickets.Application.Validators.Users;

namespace Validators.Tests.Users.Create
{
    public class CreateUserValidatorTests
    {

        [Fact]
        public void Should_NotHave_ValidationError_When_Request_Is_Correct()
        {
            // Arr
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();

            // Act
            var result = validator.TestValidate(request);

            // Ass
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Have_ValidationError_When_Name_Is_Empty(string name)
        {
            // Arr
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            request.Name = name;

            // Act
            var result = validator.TestValidate(request);

            // Ass
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("O nome do usuário é obrigatório.");
        }

        [Fact]
        public void Should_NotHave_ValidationError_When_Name_Is_Valid()
        {
            // Arr
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();

            // Act
            var result = validator.TestValidate(request);

            // Ass
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_NotHave_ValidationError_When_RoleId_Is_Correct(int roleId)
        {
            // Arr
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            request.RoleID = roleId;

            // Act
            var result = validator.TestValidate(request);

            // Ass
            result.ShouldNotHaveValidationErrorFor(x => x.RoleID);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void Should_Have_ValidationError_When_RoleId_Is_Invalid(int roleId)
        {
            // Arr
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            request.RoleID = roleId;

            // Act
            var result = validator.TestValidate(request);

            // Ass
            result.ShouldHaveValidationErrorFor(x => x.RoleID)
                .WithErrorMessage("O tipo do usuário deve ser 1 (Admin), 2 (Technician) ou 3 (User)."); 
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Have_ValidationError_When_Email_Is_Empty(string email)
        {
            // Arr
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            request.Email = email;

            // Act
            var result = validator.TestValidate(request);

            // Ass
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("O email do usuário é obrigatório.");
        }

        [Fact]
        public void Should_Have_ValidationError_When_Email_Is_Incorrect()
        {
            // Arr
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();
            request.Email = "email";

            // Act
            var result = validator.TestValidate(request);

            // Ass
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("O email do usuário deve ser um endereço de email válido.");
        }

        [Fact]
        public void Should_NotHave_ValidationError_When_Email_Is_Valid()
        {
            // Arr
            var validator = new CreateUserValidator();
            var request = CreateUserRequestBuilder.Build();

            // Act
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }
    }
}
