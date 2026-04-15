using Moq;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UserRepositoryBuilder
    {
        public static IUserRepository Build()
        {
            var mock = new Mock<IUserRepository>();
            return mock.Object;

        }
    }
}
