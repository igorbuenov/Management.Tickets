namespace Tickets.Application.Events.Users
{
    public record PasswordRecoveryEmailEvent(
        string Email, 
        string Name, 
        string TemporaryPassword);
    
}
