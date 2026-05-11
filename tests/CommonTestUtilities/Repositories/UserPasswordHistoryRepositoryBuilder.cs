using Moq;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UserPasswordHistoryRepositoryBuilder
    {
        public static IUserPasswordHistoryRepository Build()
        {
            var mock = new Mock<IUserPasswordHistoryRepository>();

            mock.Setup(repo => repo.GetAllByUserId(It.IsAny<int>()))
                .ReturnsAsync(new List<UserPasswordHistory>());

            return mock.Object;
        }
    }
}
