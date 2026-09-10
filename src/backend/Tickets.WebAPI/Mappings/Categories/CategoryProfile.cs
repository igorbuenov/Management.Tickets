using AutoMapper;
using Tickets.Application.DTOs.Categories;
using Tickets.Application.DTOs.Common;
using Tickets.WebAPI.Models.Categories;
using Tickets.WebAPI.Models.Categories.Request;
using Tickets.WebAPI.Models.Categories.Response;


namespace Tickets.WebAPI.Mappings.Categories
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, CategoryModel>();
            CreateMap<CreateCategoryRequestModel, CreateCategoryRequestDto>();
            CreateMap<CreateCategoryResponseDto, CreateCategoryResponseModel>();
            CreateMap<GetCategoriesRequestModel, GetCategoriesRequestDto>();
            CreateMap<PagedResultDto<CategoryDto>, GetCategoriesResponseModel>();
        }
    }
}
