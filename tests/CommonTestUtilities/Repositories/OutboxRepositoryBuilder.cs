using Moq;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class OutboxRepositoryBuilder
    {
        private readonly Mock<IOutboxRepository> _mock;

        public OutboxRepositoryBuilder()
        {
            _mock = new Mock<IOutboxRepository>();
        }

        public IOutboxRepository Build()
        {
            _mock.Setup(r => r.Add(It.IsAny<OutboxMessage>()))
            .Returns(Task.CompletedTask);

            return _mock.Object;
        }
    }
}
