using Tickets.Application.DTOs.Categories;
using Tickets.Domain.Entities;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Categories.CreateCategory
{
    public class CreateCategoryUseCase : ICreateCategorytUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateCategoryResponseDto> Execute(CreateCategoryRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
                throw new ErrorOnValidationException("O nome deve ser preenchido");

            Category category = new Category
            {
                Name = dto.Name,
            };

            category = await _categoryRepository.Add(category);
            await _unitOfWork.Commit();

            return BuildResponse(category);
        }


        private CreateCategoryResponseDto BuildResponse(Category department)
        {
            return new CreateCategoryResponseDto
            {
                Success = true,
                Category = new CategoryDto
                {
                    Id = department.Id,
                    CreatedAt = department.CreatedAt,
                    UpdatedAt = department.UpdatedAt,
                    Name = department.Name
                }
            };
        }
    }
}
