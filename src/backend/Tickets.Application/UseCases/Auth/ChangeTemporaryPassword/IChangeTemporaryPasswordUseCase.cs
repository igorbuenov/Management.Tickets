using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.DTOs.Auth;

namespace Tickets.Application.UseCases.Auth.ChangeTemporaryPassword
{
    public interface IChangeTemporaryPasswordUseCase
    {
        Task Execute(ChangeTemporaryPasswordRequestDto request, int id);
    }
}
