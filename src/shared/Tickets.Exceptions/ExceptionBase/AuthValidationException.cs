using System.Net;

namespace Tickets.Exceptions.ExceptionBase
{
    public class AuthValidationException : TicketsException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        
        public AuthValidationException(string message) : base(message) { }
        public AuthValidationException(List<string> errors) : base(errors) { }

    }
}
