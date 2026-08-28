namespace Tickets.WebAPI.Models.Tickets.Response
{
    public class GetTicketsResponseModel
    {
        public IEnumerable<TicketModel> Items { get; set; } = new List<TicketModel>();

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages => PageSize == 0 ? 0 :
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
