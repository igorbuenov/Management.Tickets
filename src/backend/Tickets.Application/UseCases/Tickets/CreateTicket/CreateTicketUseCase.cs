using Tickets.Application.DTOs.Categories;
using Tickets.Application.DTOs.Departments;
using Tickets.Application.DTOs.Tickets;
using Tickets.Application.DTOs.Users;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;
using Tickets.Domain.Enums;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Tickets.CreateTicket
{
    public class CreateTicketUseCase : ICreateTicketUseCase
    {

        private readonly ITicketRepository _ticketRepository;
        private readonly IUserDepartmentRepository _userDepartmentRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicketUseCase(ITicketRepository ticketRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork, IUserDepartmentRepository userDepartmentRepository)
        {
            _ticketRepository = ticketRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _userDepartmentRepository = userDepartmentRepository;
        }

        public async Task<CreateTicketResponseDto> Execute(CreateTicketRequestDto dto)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
                throw new UnauthorizedException("User must be authenticated to create tickets.");

            if (!await _userDepartmentRepository.UserBelongsToDepartment(userId.Value , dto.DepartmentId))
                throw new BusinessRuleException("O usuário não pertence ao departamento selecionado.");

            Ticket ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = TicketStatus.Open,
                CreatedAt = DateTime.Now,
                DepartmentId = dto.DepartmentId,
                CategoryId = dto.CategoryId,
                CreatedByUserId = (int) userId!
            };

            await _ticketRepository.AddAsync(ticket);
            await _unitOfWork.Commit();

            var createdTicket = await _ticketRepository.GetById(ticket.Id);

            return BuildResponse(createdTicket);
        }

        public CreateTicketResponseDto BuildResponse(Ticket ticket)
        {
            return new CreateTicketResponseDto
            {
                Success = true,
                Ticket = new TicketDto
                {
                    Title = ticket.Title,
                    Description = ticket.Description,
                    Priority = ticket.Priority.ToString(),
                    Status = ticket.Status.ToString(),
                    CreatedAt = ticket.CreatedAt,
                    UpdatedAt = ticket.UpdatedAt,
                    Category = new CategoryDto
                    {
                        Id = ticket.CategoryId,
                        Name = ticket.Category.Name, 
                    },
                    Department = new DepartmentDto
                    {
                        Id = ticket.DepartmentId,
                        Name = ticket.Department.Name,
                    },
                    CreatedBy = new UserSummaryDto
                    {
                        Id = ticket.CreatedByUser.Id,
                        Name = ticket.CreatedByUser.Name
                    },
                    AssignedTo = ticket.AssignedToUser == null
                    ? null
                    : new UserSummaryDto
                    {
                        Id = ticket.AssignedToUser.Id,
                        Name = ticket.AssignedToUser.Name
                    }
                }
            };
        }
    }
}
