namespace Tickets.Application.Events.Users
{
    public record CreateUserEmailEvent(
        string Email, 
        string Name, 
        string Password);
    
}
