namespace Tickets.Exceptions.ExceptionBase
{
    public class BusinessRuleException : TicketsException
    {
        public List<string> Errors { get; set; } = new List<string>();

        public BusinessRuleException(string message)
        {
            Errors.Add(message);
        }

        public BusinessRuleException(List<string> messages)
        {
            Errors = messages;
        }
    }
}
