namespace Tickets.Exceptions.ExceptionBase
{
    public class UnauthorizedException : TicketsException
    {
        public List<string> Errors { get; set; } = new List<string>();

        public UnauthorizedException(string message)
        {
            Errors.Add(message);
        }

        public UnauthorizedException(List<string> messages)
        {
            Errors = messages;
        }
    }
}
