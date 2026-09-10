namespace Tickets.WebAPI.Models.Categories.Response
{
    public class CreateCategoryResponseModel
    {
        public bool Success { get; set; }
        public CategoryModel Category { get; set; }
    }
}
