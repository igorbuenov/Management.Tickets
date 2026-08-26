using Moq;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UserRoleRepositoryBuilder
    {

        private readonly Mock<IUserRoleRepository> _repository;

        public UserRoleRepositoryBuilder() => _repository = new Mock<IUserRoleRepository>();

        public IUserRoleRepository Build()
        {
            _repository.Setup(r => r.Add(It.IsAny<int>(), It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            return _repository.Object;
        }

        public void GetRoleByID()
        {
            _repository.Setup(r => r.GetRoleByID(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return id == 1 || id == 2 || id == 3
                        ? new Role { Id = id, Description = id == 1 ? "Admin" : id == 2 ? "Technician" : "User" }
                        : null;
                });
        }

        public void GetRoleByID(int roleId)
        {
            _repository.Setup(r => r.GetRoleByID(roleId))
                .ReturnsAsync((Role?)null);
        }    
    }
}
