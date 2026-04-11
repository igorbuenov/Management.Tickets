using AutoMapper;
using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Users;
using Tickets.WebAPI.Models.Users.Request;
using Tickets.WebAPI.Models.Users.Response;

namespace Tickets.WebAPI.Mappings.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserRequestModel, CreateUserDto>();
            CreateMap<CreateUserResponseDto, CreateUserResponseModel>();
            CreateMap<PagedResultDto<UserDto>, GetUsersResponseModel<UserDto>>();
            CreateMap<UserDto, UserResponseModel>();
            CreateMap<UpdateUserRequestModel, UpdateUserDto>();
            CreateMap<UpdatePasswordRequestModel, UpdatePasswordRequestDto>();
        }
    }
}
