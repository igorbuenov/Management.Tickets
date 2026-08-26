using Moq;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class PasswordRepositoryBuilder
    {
        public static IPasswordRepository Build()
        {
            var mock = new Mock<IPasswordRepository>();

            mock.Setup(repo => repo.Add(It.IsAny<UserPassword>()))
                .Returns(Task.CompletedTask);

            mock.Setup(repo => repo.GetByUserId(It.IsAny<int>()))
                .ReturnsAsync((int userId) => new UserPassword
                {
                    UserId = userId,
                    HashPassword = "hashed_password"
                });

            return mock.Object;
        }
    }
}
