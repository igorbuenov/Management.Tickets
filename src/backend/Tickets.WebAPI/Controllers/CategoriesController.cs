using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTOs.Categories;
using Tickets.Application.UseCases.Categories.CreateCategory;
using Tickets.Application.UseCases.Categories.GetCategories;
using Tickets.WebAPI.Models.Categories.Request;
using Tickets.WebAPI.Models.Categories.Response;

namespace Tickets.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICreateCategorytUseCase _createCategorytUseCase;
        private readonly IGetCategoriesUseCase _getCategoriesUseCase;

        public CategoriesController(IMapper mapper, ICreateCategorytUseCase createCategorytUseCase, IGetCategoriesUseCase getCategoriesUseCase)
        {
            _mapper = mapper;
            _createCategorytUseCase = createCategorytUseCase;
            _getCategoriesUseCase = getCategoriesUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestModel request)
        {
            var response = _mapper.Map<CreateCategoryResponseModel>(await _createCategorytUseCase.Execute(_mapper.Map<CreateCategoryRequestDto>(request)));
            return Created(string.Empty, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesRequestModel request)
        {
            var response = _mapper.Map<GetCategoriesResponseModel>(await _getCategoriesUseCase.Execute(_mapper.Map<GetCategoriesRequestDto>(request)));
            return Ok(response);
        }

    }
}
