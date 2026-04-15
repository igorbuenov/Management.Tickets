using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Tickets.Domain.Interfaces.Repositories;

namespace CommonTestUtilities.Repositories
{
    public class PasswordRepositoryBuilder
    {
        public static IPasswordRepository Build()
        {
            var mock = new Mock<IPasswordRepository>();
            return mock.Object;
        }
    }
}
