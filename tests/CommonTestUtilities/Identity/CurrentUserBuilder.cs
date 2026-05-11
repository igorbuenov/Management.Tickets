using Moq;
using Tickets.Application.Interfaces;

namespace CommonTestUtilities.Identity
{
    public class CurrentUserBuilder
    {
        public static ICurrentUser Build()
        {
            var mock = new Mock<ICurrentUser>();
            mock.SetupGet(x => x.UserId).Returns(1);
            return mock.Object;
        }
    }
}
