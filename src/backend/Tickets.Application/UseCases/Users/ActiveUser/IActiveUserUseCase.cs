namespace Tickets.Application.UseCases.Users.ActiveUser
{
    public interface IActiveUserUseCase
    {
        Task Execute(int id);
    }
}
