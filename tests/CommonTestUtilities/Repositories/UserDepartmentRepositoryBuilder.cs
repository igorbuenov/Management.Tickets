using Moq;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UserDepartmentRepositoryBuilder
    {
        private readonly Mock<IUserDepartmentRepository> _mock;

        public UserDepartmentRepositoryBuilder()
        {
            _mock = new Mock<IUserDepartmentRepository>();
        }

        public IUserDepartmentRepository Build()
        {
            _mock.Setup(r => r.Add(It.IsAny<UserDepartment>()))
                .ReturnsAsync((UserDepartment department) => department);

            return _mock.Object;
        }
    }
}
