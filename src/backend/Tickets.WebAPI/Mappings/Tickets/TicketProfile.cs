using AutoMapper;
using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.DTOs.Users;
using Tickets.WebAPI.Models.Tickets;
using Tickets.WebAPI.Models.Tickets.Request;
using Tickets.WebAPI.Models.Tickets.Response;
using Tickets.WebAPI.Models.Users;

namespace Tickets.WebAPI.Mappings.Tickets
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<CreateTicketRequestModel, CreateTicketRequestDto>();
            CreateMap<CreateTicketResponseDto, CreateTicketResponseModel>();
            CreateMap<TicketDto, TicketModel>();
            CreateMap<UserSummaryDto, UserSummaryModel>();
            CreateMap<PagedResultDto<TicketDto>, GetTicketsResponseModel>();
        }
    }
}
