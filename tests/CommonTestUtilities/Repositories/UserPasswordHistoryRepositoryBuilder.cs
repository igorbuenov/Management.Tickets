using Moq;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UserPasswordHistoryRepositoryBuilder
    {
        private readonly Mock<IUserPasswordHistoryRepository> _mock;

        public UserPasswordHistoryRepositoryBuilder()
        {
            _mock = new Mock<IUserPasswordHistoryRepository>();
        }

        public IUserPasswordHistoryRepository Build()
        {
            _mock.Setup(r => r.Add(It.IsAny<UserPasswordHistory>()))
                .Returns(Task.CompletedTask);

            return _mock.Object;
        }
    }
}
