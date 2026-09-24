using Moq;
using Tickets.Application.Interfaces;

namespace CommonTestUtilities.Interfaces
{
    public class PasswordServiceBuilder
    {
        private readonly Mock<IPasswordService> _mock;

        public PasswordServiceBuilder()
        {
            _mock = new Mock<IPasswordService>();
        }

        public IPasswordService Build()
        {
            return _mock.Object;
        }

        public void GeneratePassword(string password)
        {
            _mock
                .Setup(x => x.GenerateRandomPassword())
                .Returns(password);
        }

        public void HashPassword(string password, string hash)
        {
            _mock
                .Setup(x => x.HashPassword(password))
                .Returns(hash);
        }

    }
}
