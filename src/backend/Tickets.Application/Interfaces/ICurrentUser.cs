namespace Tickets.Application.Interfaces
{
    public interface ICurrentUser
    {
        int? UserId { get;}
        string? Email { get;}
        string? Role { get; }
        bool IsAuthenticated { get; }
    }
}
