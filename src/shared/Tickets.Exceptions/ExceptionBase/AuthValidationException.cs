namespace Tickets.Exceptions.ExceptionBase
{
    public class AuthValidationException : TicketsException
    {
        public string errorMessage { get; set; }

        public AuthValidationException(string message)
        {
            errorMessage = message;
        }
    }
}
