using Moq;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UserRepositoryBuilder
    {
        private readonly Mock<IUserRepository> _repository;

        public UserRepositoryBuilder()
        {
            _repository = new Mock<IUserRepository>();
        } 

        public IUserRepository Build()
        {
            _repository.Setup(r => r.Add(It.IsAny<User>()))
                .ReturnsAsync((User u) =>
                {
                    u.Id = u.Id == 0 ? 1 : u.Id;
                    return u;
                });

            _repository.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync
                ((int id) => id == 0 
                        ? null 
                        : new User { Id = id, Name = "User", Email = "user@mail.com", IsActive = true }
                );

            return _repository.Object;
        }


        public void UserEmailDoesNotExist()
        {
            _repository
                .Setup(r => r.ExistActiveUserWithEmail(It.IsAny<string>()))
                .ReturnsAsync(false);
        }

        public void UserEmailAlreadyExists(string email)
        {
            _repository
                .Setup(r => r.ExistActiveUserWithEmail(email))
                .ReturnsAsync(true);
        }



    }
}