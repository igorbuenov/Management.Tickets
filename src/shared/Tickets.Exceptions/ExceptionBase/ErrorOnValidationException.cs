using System.Net;

namespace Tickets.Exceptions.ExceptionBase
{
    public class ErrorOnValidationException : TicketsException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public ErrorOnValidationException(string message) : base(message) { }
        public ErrorOnValidationException(List<string> messages) : base(messages) { }
    }
}
