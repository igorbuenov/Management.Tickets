using System.Net;

namespace Tickets.Exceptions.ExceptionBase
{
    public class ForbiddenException : TicketsException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

        public ForbiddenException(string message) : base(message) { }
        public ForbiddenException(List<string> messages) : base(messages) { }
    }
}
