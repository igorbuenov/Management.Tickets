namespace Tickets.Exceptions.ExceptionBase
{
    public class ErrorOnValidationException : TicketsException
    {
        public IList<string> ErrorMessages { get; set; }

        public ErrorOnValidationException(IList<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}
