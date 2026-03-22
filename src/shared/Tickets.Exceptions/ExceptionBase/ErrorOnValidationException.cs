namespace Tickets.Exceptions.ExceptionBase
{
    public class ErrorOnValidationException : TicketsException
    {
        public List<string> Errors { get; set; } = new List<string>();

        public ErrorOnValidationException(List<string> errorMessages)
        {
            Errors = errorMessages;
        }
    }
}
