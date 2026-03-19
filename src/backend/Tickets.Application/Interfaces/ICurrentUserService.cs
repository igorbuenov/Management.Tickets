namespace Tickets.Application.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get;}
        string? Email { get;}
    }
}
