using System.Net;

namespace Tickets.Exceptions.ExceptionBase
{
    public class BusinessRuleException : TicketsException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public BusinessRuleException(string message) : base(message) { }
        public BusinessRuleException(List<string> messages) : base(messages) { }
        
    }
}
