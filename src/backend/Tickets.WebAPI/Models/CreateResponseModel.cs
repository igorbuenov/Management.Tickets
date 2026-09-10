namespace Tickets.WebAPI.Models
{
    public class CreateResponseModel<T>
    {
        public bool Success { get; set; }
        public T Item { get; set; }
    }
}
