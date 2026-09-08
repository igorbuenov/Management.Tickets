using Tickets.Application.DTOs.Tickets;

namespace Tickets.Application.UseCases.Tickets.GetTicketById
{
    public interface IGetTicketByIdUseCase
    {
        Task<TicketDto> Execute(int id);
    }
}
