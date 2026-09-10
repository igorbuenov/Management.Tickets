using Tickets.Application.DTOs.Categories;
using Tickets.Application.DTOs.Common;

namespace Tickets.Application.UseCases.Categories.GetCategories
{
    public interface IGetCategoriesUseCase
    {
        Task<PagedResultDto<CategoryDto>> Execute(GetCategoriesRequestDto dto);
    }
}
