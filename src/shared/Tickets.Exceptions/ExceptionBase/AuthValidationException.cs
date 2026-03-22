namespace Tickets.Exceptions.ExceptionBase
{
    public class AuthValidationException : TicketsException
    {
        public List<string> Errors { get; set; } = new List<string>();

        public AuthValidationException(string message)
        {
            Errors.Add(message);
        }

        public AuthValidationException(List<string> messages)
        {
            Errors = messages;
        }
    }
}
