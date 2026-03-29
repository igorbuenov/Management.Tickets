namespace Tickets.Exceptions.ExceptionBase
{
    public class ForbiddenException : TicketsException
    {
        public List<string> Errors { get; set; } = new List<string>();

        public ForbiddenException(string message)
        {
            Errors.Add(message);
        }

        public ForbiddenException(List<string> messages)
        {
            Errors = messages;
        }
    }
}
