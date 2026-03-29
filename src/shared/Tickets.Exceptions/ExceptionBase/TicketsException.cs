using System.Net;

namespace Tickets.Exceptions.ExceptionBase
{
    public abstract class TicketsException : Exception
    {
        public abstract HttpStatusCode StatusCode { get; }

        public List<string> Errors { get; }

        protected TicketsException(string message)
            : base(message)
        {
            Errors = new List<string> { message };
        }

        protected TicketsException(List<string> errors)
            : base(errors.FirstOrDefault() ?? "An error occurred.")
        {
            Errors = errors?.Any() == true
                ? errors
                : new List<string> { "An error occurred." };
        }
    }
}