namespace Tickets.WebAPI.Exceptions.Handlers
{
    public class ExceptionResponse
    {
        public int StatusCode { get; set; }
        public object Body { get; set; } = default!;
    }
}
