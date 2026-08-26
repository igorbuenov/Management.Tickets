using Moq;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            return mock.Object;
        }
    }
}
