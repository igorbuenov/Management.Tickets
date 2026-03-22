using AutoMapper;
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
            // CreateMap<UpdateUserRequestModel, UpdateUserDTO>();
            // CreateMap<User, UserResponseModel>();
        }
    }
}
