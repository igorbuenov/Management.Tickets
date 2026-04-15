using Moq;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UserRoleRepositoryBuilder
    {
        public static IUserRoleRepository Build()
        {
            var mock = new Mock<IUserRoleRepository>();
            return mock.Object;
        }
    }
}
