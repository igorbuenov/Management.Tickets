using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTOs.Users;
using Tickets.Application.UseCases.Users.CreateUser;
using Tickets.WebAPI.Models.Users.Request;
using Tickets.WebAPI.Models.Users.Response;


namespace Tickets.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ICreateUserUseCase _createUserUseCase;

        public UsersController(IMapper mapper, ICreateUserUseCase createUserUseCase)
        {
            _mapper = mapper;
            _createUserUseCase = createUserUseCase;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel request)
        {

            // Mapper Request to DTO
            CreateUserDto createUserDto = _mapper.Map<CreateUserDto>(request);

            int userId = await _createUserUseCase.Execute(createUserDto);

            CreateUserResponseModel response = new CreateUserResponseModel
            {
                UserId = userId
            };

            return Created(string.Empty, response);
        }



    }
}
