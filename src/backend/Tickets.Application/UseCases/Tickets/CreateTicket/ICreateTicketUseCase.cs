using Tickets.Application.DTOs.Tickets;

namespace Tickets.Application.UseCases.Tickets.CreateTicket
{
    public interface ICreateTicketUseCase
    {
        Task<CreateTicketResponseDto> Execute(CreateTicketRequestDto dto);
    }
}
