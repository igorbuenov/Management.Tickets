using Moq;
using Tickets.Application.Interfaces;

namespace CommonTestUtilities.Interfaces
{
    public class PasswordServiceBuilder
    {
        public static IPasswordService Build()
        {
            var mock = new Mock<IPasswordService>();
            return mock.Object;
        }
    }
}
