using Tickets.Application.DTOs.Categories;
using Tickets.Application.DTOs.Common;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Categories.GetCategories
{
    public class GetCategoriesUseCase : IGetCategoriesUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoriesUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResultDto<CategoryDto>> Execute(GetCategoriesRequestDto request)
        {

            if (request.Page <= 0)
                throw new ErrorOnValidationException("Page must be greater than 0");

            if (request.PageSize <= 0)
                throw new ErrorOnValidationException("PageSize must be greater than 0");

            var categories = await _categoryRepository.GetPaged(request.Page, request.PageSize, request.Name);
            var totalCategories = await _categoryRepository.Count(request.Name);

            return BuildResponse(categories, request.Page, request.PageSize, totalCategories);
        }

        private PagedResultDto<CategoryDto> BuildResponse(IEnumerable<Category> departments, int page, int pageSize ,int total)
        {
            return new PagedResultDto<CategoryDto>
            {
                Items = departments.Select(department => new CategoryDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    CreatedAt = department.CreatedAt,
                    UpdatedAt = department.UpdatedAt
                }).ToList(),

                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }
    }
}
