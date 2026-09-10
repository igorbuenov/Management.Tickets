using Tickets.Application.DTOs.Categories;

namespace Tickets.Application.UseCases.Categories.CreateCategory
{
    public interface ICreateCategorytUseCase
    {
        Task<CreateCategoryResponseDto> Execute(CreateCategoryRequestDto dto);
    }
}
