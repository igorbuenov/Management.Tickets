using AutoMapper;
using Tickets.Application.DTOs.Auth;
using Tickets.WebAPI.Models.RequestModels.Auth.Request;

namespace Tickets.WebAPI.Mappings.Auth
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<LoginRequestModel, LoginRequestDto>();
        }
    }
}
