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

namespace UseCases.Tests.Users.CreateUser
{
    public class CreateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            // Arr
            var userRepository = UserRepositoryBuilder.Build();
            var passwordRepository = PasswordRepositoryBuilder.Build();
            var userPasswordHistoryRepository = UserPasswordHistoryRepositoryBuilder.Build();
            var userRoleRepository = UserRoleRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var passwordService = PasswordServiceBuilder.Build();
            var userEmailService = UserEmailServiceBuilder.Build();
            var currentUser = CurrentUserBuilder.Build();
            var logger = LoggerBuilder.Build<CreateUserUseCase>();
            var validator = ValidatorBuilder.Build<CreateUserRequestDto>();

            var request = CreateUserRequestBuilder.Build();
            var useCase = new CreateUserUseCase(
                userRepository,
                passwordService, 
                passwordRepository,
                unitOfWork,
                userRoleRepository,
                currentUser,
                logger,
                userEmailService,
                userPasswordHistoryRepository,
                validator);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.Should().NotBeNull();
            result.User.Name.Should().Be(request.Name);

        }
    }
}
