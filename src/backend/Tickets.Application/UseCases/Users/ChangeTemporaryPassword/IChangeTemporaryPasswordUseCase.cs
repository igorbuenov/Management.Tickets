using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.DTOs.Users;

namespace Tickets.Application.UseCases.Users.ChangeTemporaryPassword
{
    public interface IChangeTemporaryPasswordUseCase
    {
        Task Execute(ChangeTemporaryPasswordRequestDto request, int id);
    }
}
