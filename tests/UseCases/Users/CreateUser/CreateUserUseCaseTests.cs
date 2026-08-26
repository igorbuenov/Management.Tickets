using CommonTestUtilities.Identity;
using CommonTestUtilities.Interfaces;
using CommonTestUtilities.Logging;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using CommonTestUtilities.Validator;
using FluentAssertions;
using Tickets.Application.DTOs.Users;
using Tickets.Application.UseCases.Users.CreateUser;
using Tickets.Exceptions.ExceptionBase;

namespace UseCases.Tests.Users.CreateUser
{
    public class CreateUserUseCaseTests
    {
        [Fact]
        public async Task Should_Create_User_When_Request_IsValid()
        {
            // Arr
            var request = CreateUserRequestBuilder.Build();

            var useCase = CreateUseCase();

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.Success.Should().BeTrue();

        }

        [Fact]
        public async Task Throw_Exception_When_Email_Already_Registered()
        {
            // Arr
            var request = CreateUserRequestBuilder.Build();
            var useCase = CreateUseCase(request.Email);

            // Act
            Func<Task> act = async () => await useCase.Execute(request);

            // Assert
            await act.Should()
                .ThrowAsync<ErrorOnValidationException>()
                .WithMessage("E-mail já está em uso");

        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task Throw_Exception_When_RoleID_IsInvalid(int roleId)
        {
            // Arr
            var request = CreateUserRequestBuilder.Build();
            request.RoleID = roleId;
            var useCase = CreateUseCase(null, roleId);

            // Act
            Func<Task> act = async () => await useCase.Execute(request);

            // Assert
            await act.Should()
                .ThrowAsync<ErrorOnValidationException>()
                .WithMessage("RoleID inválido");
            
        }


        private CreateUserUseCase CreateUseCase(
            string? email = null,
            int? roleId = null)
        {
            var userRepositoryBuilder = new UserRepositoryBuilder();
            var passwordRepository = PasswordRepositoryBuilder.Build();
            var userPasswordHistoryRepository = UserPasswordHistoryRepositoryBuilder.Build();
            var userRoleRepository = new UserRoleRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var passwordService = PasswordServiceBuilder.Build();
            var userEmailService = UserEmailServiceBuilder.Build();
            var currentUser = CurrentUserBuilder.Build();
            var logger = LoggerBuilder.Build<CreateUserUseCase>();
            var validator = ValidatorBuilder.Build<CreateUserRequestDto>();


            // Setup for the tests
            userRepositoryBuilder.ExistActiveUserWithEmail();
            userRoleRepository.GetRoleByID();

            // Specific setups for each test
            if (!string.IsNullOrEmpty(email))
                userRepositoryBuilder.ExistActiveUserWithEmail(email);

            if (roleId.HasValue)
                userRoleRepository.GetRoleByID(roleId.Value);


            return new CreateUserUseCase(
                userRepositoryBuilder.Build(),
                passwordService,
                passwordRepository,
                unitOfWork,
                userRoleRepository.Build(),
                currentUser,
                logger,
                userEmailService,
                UserPasswordHistoryRepositoryBuilder.Build(),  
                validator);
        }
    }
}
