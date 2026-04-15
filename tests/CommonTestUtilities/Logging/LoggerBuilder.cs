using Microsoft.Extensions.Logging;
using Moq;

namespace CommonTestUtilities.Logging
{
    public class LoggerBuilder
    {
        public static ILogger<T> Build<T>()
        {
            var mock = new Mock<ILogger<T>>();
            return mock.Object;
        }
    }
}
