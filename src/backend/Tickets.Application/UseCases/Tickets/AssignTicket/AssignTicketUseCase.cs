using Microsoft.Extensions.Logging;
using Tickets.Application.DTOs.Tickets;
using Tickets.Domain.Enums;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Tickets.AssignTicket
{
    public class AssignTicketUseCase : IAssignTicketUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignTicketUseCase> _logger;

        public AssignTicketUseCase(
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IUnitOfWork unitOfWork,
            ILogger<AssignTicketUseCase> logger)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(int ticketId, AssignTicketRequestDto request)
        {
            _logger.LogInformation("Iniciando atribuição do Ticket {TicketId} para o usuário {TechnicianId}", ticketId, request.TechnicianId);

            var ticket = await _ticketRepository.GetById(ticketId);
            if (ticket == null)
                throw new NotFoundException("Ticket não encontrado.");

            var technician = await _userRepository.GetById(request.TechnicianId);
            if (technician == null)
                throw new NotFoundException("Técnico não encontrado.");

            if (ticket.CreatedByUserId == technician.Id)
                throw new BusinessRuleException("Não é permitido atender tickets que o pertencem!");

            var roles = await _userRoleRepository.GetRolesByUserId(technician.Id);

            if (!roles.Any(role => 
                role.Id.Equals((int)UserRoleEnum.Technician) ||
                role.Id.Equals((int)UserRoleEnum.Admin)))
            {
                throw new BusinessRuleException("O usuário selecionado não possui o perfil de técnico.");
            }
                
            ticket.AssignedToUserId = technician.Id;
            ticket.Status = TicketStatus.InProgress; 
            ticket.UpdatedAt = DateTime.Now;

            await _unitOfWork.Commit();

            _logger.LogInformation("Ticket {TicketId} atribuído com sucesso para o técnico {TechnicianId}", ticketId, request.TechnicianId);
        }
    }
}

