using AutoMapper;
using Tickets.Application.DTOs.Auth;
using Tickets.WebAPI.Models.Auth;
using Tickets.WebAPI.Models.Auth.Request;
using Tickets.WebAPI.Models.Auth.Response;

namespace Tickets.WebAPI.Mappings.Auth
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<LoginRequestModel, LoginRequestDto>();
            CreateMap<LoginResponseDto, LoginResponseModel>();
            CreateMap<LoginUserDto, LoginUserModel>();
        }
    }
}
