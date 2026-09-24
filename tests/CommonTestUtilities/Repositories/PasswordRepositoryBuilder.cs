using Moq;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class PasswordRepositoryBuilder
    {
        private readonly Mock<IPasswordRepository> _mock;

        public PasswordRepositoryBuilder()
        {
            _mock = new Mock<IPasswordRepository>();
        }

        public IPasswordRepository Build()
        {

            _mock.Setup(repo => repo.Add(It.IsAny<UserPassword>()))
                .Returns(Task.CompletedTask);

            return _mock.Object;
        }
    }
}
