using Tickets.Application.DTOs.Departments;
using Tickets.Application.Interfaces;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Exceptions.ExceptionBase;

namespace Tickets.Application.UseCases.Departments.GetMyDepartments
{
    public class GetMyDepartmentsUseCase : IGetMyDepartmentsUseCase
    {
        private readonly IUserDepartmentRepository _userDepartmentRepository;
        private readonly ICurrentUser _currentUser;

        public GetMyDepartmentsUseCase(
            IUserDepartmentRepository userDepartmentRepository,
            ICurrentUser currentUser)
        {
            _userDepartmentRepository = userDepartmentRepository;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<DepartmentDto>> Execute()
        {
            var userId = _currentUser.UserId;

            if (userId == null)
                throw new UnauthorizedException(
                    "User must be authenticated to get departments.");

            var departments =
                await _userDepartmentRepository.GetDepartmentsByUserId(userId.Value);

            return departments.Select(department => new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name
            });
        }
    }
}
