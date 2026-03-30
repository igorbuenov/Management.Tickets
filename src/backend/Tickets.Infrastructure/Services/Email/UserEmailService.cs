using Tickets.Application.Interfaces;

namespace Tickets.Infrastructure.Services.Email
{
    public class UserEmailService : IUserEmailService
    {

        private readonly IEmailService _emailService;

        public UserEmailService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendWelcomeEmailAsync(
            string email,
            string name,
            string temporaryPassword)
        {
            var subject = "Bem-vindo ao Management Tickets";

            var html = $@"
                <h2>Olá {name}</h2>
                <p>Sua conta foi criada com sucesso.</p>
                <p><b>Senha temporária:</b> {temporaryPassword}</p>
                <p>Essa senha deve ser alterada no primeiro acesso.</p>";

            await _emailService.SendAsync(email, name, subject, html);
        }
    }
}
