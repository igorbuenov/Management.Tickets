using Moq;
using Tickets.Application.Interfaces;

namespace CommonTestUtilities.Identity
{
    public class CurrentUserBuilder
    {
        public static ICurrentUser Build()
        {
            var mock = new Mock<ICurrentUser>();
            return mock.Object;
        }
    }
}
