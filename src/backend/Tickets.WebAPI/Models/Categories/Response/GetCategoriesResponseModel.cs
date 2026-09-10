namespace Tickets.WebAPI.Models.Categories.Response
{
    public class GetCategoriesResponseModel
    {
        public IEnumerable<CategoryModel> Items { get; set; } = new List<CategoryModel>();

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages => PageSize == 0 ? 0 :
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
