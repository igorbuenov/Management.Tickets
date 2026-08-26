using Moq;
using Tickets.Application.Interfaces;

namespace CommonTestUtilities.Services
{
    public class UserEmailServiceBuilder
    {
        public static IUserEmailService Build()
        {
            var mock = new Mock<IUserEmailService>();
            return mock.Object;
        }
    }
}
