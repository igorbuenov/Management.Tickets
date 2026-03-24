namespace Tickets.Exceptions.ExceptionBase
{
    public class NotFoundException : TicketsException
    {
        public List<string> Errors { get; set; } = new List<string>();

        public NotFoundException(string message)
        {
            Errors.Add(message);
        }

        public NotFoundException(List<string> messages)
        {
            Errors = messages;
        }
    }
}
