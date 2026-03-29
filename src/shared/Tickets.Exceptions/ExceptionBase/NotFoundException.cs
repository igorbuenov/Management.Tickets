using System.Net;

namespace Tickets.Exceptions.ExceptionBase
{
    public class NotFoundException : TicketsException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public NotFoundException(string message) : base(message) { }
        public NotFoundException(List<string> messages) : base(messages) { }
    }
}
