using Tickets.Application.DTOs.Tickets;

namespace Tickets.Application.UseCases.Tickets.CreateTicketMessage
{
    public interface ICreateTicketMessageUseCase
    {
        Task Execute(int ticketId, CreateTicketMessageRequestDto request);
    }
}
