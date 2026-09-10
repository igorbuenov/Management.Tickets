namespace Tickets.Application.DTOs.Categories
{
    public class CreateCategoryResponseDto
    {
        public bool Success { get; set; }
        public CategoryDto Category { get; set; }
    }
}
