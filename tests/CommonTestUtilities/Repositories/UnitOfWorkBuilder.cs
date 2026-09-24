using Moq;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        private readonly Mock<IUnitOfWork> _mock;
        public UnitOfWorkBuilder()
        {
            _mock = new Mock<IUnitOfWork>();
        }

        public IUnitOfWork Build()
        {
            _mock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            return _mock.Object;
        }
    }
}
