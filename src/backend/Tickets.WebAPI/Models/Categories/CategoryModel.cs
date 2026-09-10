namespace Tickets.WebAPI.Models.Categories
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Name { get; set; }
    }
}
