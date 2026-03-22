namespace Tickets.WebAPI.Models.Error.Response
{
    public class ErrorResponseModel
    {
        public bool Success => false;

        public List<string> Errors { get; set; }

        public ErrorResponseModel(List<string> errors)
        {
            Errors = errors;
        }
    }
}
