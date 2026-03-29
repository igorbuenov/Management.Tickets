using System.Net;

namespace Tickets.Exceptions.ExceptionBase
{
    public class UnauthorizedException : TicketsException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public UnauthorizedException(string message) : base(message) { }
        public UnauthorizedException(List<string> messages) : base(messages) { }
    }
}
