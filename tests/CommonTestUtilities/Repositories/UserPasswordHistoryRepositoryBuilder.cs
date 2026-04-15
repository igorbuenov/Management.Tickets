using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class UserPasswordHistoryRepositoryBuilder
    {
            public static IUserPasswordHistoryRepository Build()
            {
                var mock = new Moq.Mock<IUserPasswordHistoryRepository>();
                return mock.Object;
        }
    }
}
