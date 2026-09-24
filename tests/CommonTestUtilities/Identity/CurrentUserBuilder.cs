using Moq;
using Tickets.Application.Interfaces;

namespace CommonTestUtilities.Identity
{
    public class CurrentUserBuilder
    {
        private readonly Mock<ICurrentUser> _mock;

        public CurrentUserBuilder()
        {
            _mock = new Mock<ICurrentUser>();
        }

        public ICurrentUser Build()
        {
            _mock.SetupGet(x => x.UserId).Returns(1);
            return _mock.Object;
        }
    }
}
