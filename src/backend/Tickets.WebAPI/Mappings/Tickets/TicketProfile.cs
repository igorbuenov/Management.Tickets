using AutoMapper;
using Tickets.Application.DTOs.Common;
using Tickets.Application.DTOs.Tickets;
using Tickets.WebAPI.Models.Tickets;
using Tickets.WebAPI.Models.Tickets.Request;
using Tickets.WebAPI.Models.Tickets.Response;

namespace Tickets.WebAPI.Mappings.Tickets
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<CreateTicketRequestModel, CreateTicketRequestDto>();
            CreateMap<CreateTicketResponseDto, CreateTicketResponseModel>();
            CreateMap<TicketDto, TicketModel>();
            CreateMap<PagedResultDto<TicketDto>, GetTicketsResponseModel>();
        }
    }
}
