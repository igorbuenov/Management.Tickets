using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTOs.Users;
using Tickets.Application.UseCases.Users.CreateUser;
using Tickets.Application.UseCases.Users.GetUserById;
using Tickets.Application.UseCases.Users.GetUsers;
using Tickets.Application.UseCases.Users.UpdateUser;
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
        private readonly IGetUsersUseCase _getUsersUseCase;
        private readonly IGetUserByIdUseCase _getUserByIdUseCase;
        private readonly IUpdateUserUseCase _updateUserUseCase;

        public UsersController(IMapper mapper, ICreateUserUseCase createUserUseCase, IGetUsersUseCase getUsersUseCase, IGetUserByIdUseCase getUserByIdUseCase, IUpdateUserUseCase updateUserUseCase)
        {
            _mapper = mapper;
            _createUserUseCase = createUserUseCase;
            _getUsersUseCase = getUsersUseCase;
            _getUserByIdUseCase = getUserByIdUseCase;
            _updateUserUseCase = updateUserUseCase;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel request)
        {
            var response = _mapper.Map<CreateUserResponseModel>(await _createUserUseCase.Execute(_mapper.Map<CreateUserDto>(request)));
            return Created(string.Empty, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequestModel request)
        {
            var response = _mapper.Map<GetUsersResponseModel<UserDto>>(await _getUsersUseCase.Execute(request.Page, request.PageSize));
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = _mapper.Map<UserResponseModel>(await _getUserByIdUseCase.Execute(id));
            return Ok(response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequestModel request)
        {
            var updateuserDto = _mapper.Map<UpdateUserDto>(request);
            updateuserDto.Id = id;

            await _updateUserUseCase.Execute(updateuserDto);

            return NoContent();
        }


    }
}
