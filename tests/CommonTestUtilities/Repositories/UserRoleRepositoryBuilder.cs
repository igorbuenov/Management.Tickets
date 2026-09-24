using Moq;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UserRoleRepositoryBuilder
    {

        private readonly Mock<IUserRoleRepository> _mock;

        public UserRoleRepositoryBuilder() => _mock = new Mock<IUserRoleRepository>();

        public IUserRoleRepository Build()
        {
            _mock.Setup(r => r.Add(It.IsAny<int>(), It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            return _mock.Object;
        }

        public void RoleExists()
        {
            _mock.Setup(r => r.GetRoleByID(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return id == 1 || id == 2 || id == 3
                        ? new Role { Id = id, Description = id == 1 ? "Admin" : id == 2 ? "Technician" : "User" }
                        : null;
                });
        }

        public void RoleDoesNotExist(int roleId)
        {
            _mock.Setup(r => r.GetRoleByID(roleId))
                .ReturnsAsync((Role?)null);
        }    
    }
}
