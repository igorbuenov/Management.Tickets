using System.Net;
using Tickets.Application.Interfaces;
using Tickets.Infrastructure.Services.Email.Enums;

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

            var html = BuildHtmlEmailTemplate(
                TypeEmailEnum.WelcomeEmail,
                name,
                temporaryPassword);

            await _emailService.SendAsync(email, name, subject, html);
        }

        public async Task SendPasswordResetEmailAsync(
            string email,
            string name,
            string resetLink)
        {
            var subject = "Redefinição de senha - Management Tickets";

            var html = BuildHtmlEmailTemplate(
                TypeEmailEnum.PasswordRecoveryEmail,
                name,
                resetLink);

            await _emailService.SendAsync(email, name, subject, html);
        }

        private string BuildHtmlEmailTemplate(
            TypeEmailEnum typeEmail,
            string name,
            string content)
        {
            var safeName = WebUtility.HtmlEncode(name);
            var safeContent = WebUtility.HtmlEncode(content);

            return typeEmail switch
            {
                TypeEmailEnum.WelcomeEmail => $@"
                        <!DOCTYPE html>
                        <html lang=""pt-BR"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Bem-vindo ao Management Tickets</title>
                        </head>

                        <body style=""
                            margin: 0;
                            padding: 0;
                            background-color: #f9fafb;
                            font-family: Arial, Helvetica, sans-serif;
                            color: #111827;
                        "">

                            <table
                                width=""100%""
                                cellpadding=""0""
                                cellspacing=""0""
                                border=""0""
                                style=""
                                    background-color: #f9fafb;
                                    padding: 40px 20px;
                                "">

                                <tr>
                                    <td align=""center"">

                                        <table
                                            width=""100%""
                                            cellpadding=""0""
                                            cellspacing=""0""
                                            border=""0""
                                            style=""
                                                max-width: 560px;
                                                background-color: #ffffff;
                                                border: 1px solid #e5e7eb;
                                                border-radius: 12px;
                                            "">

                                            <tr>
                                                <td style=""padding: 40px;"">

                                                    <h1 style=""
                                                        margin: 0 0 24px 0;
                                                        font-size: 24px;
                                                        line-height: 32px;
                                                        font-weight: 700;
                                                        color: #111827;
                                                    "">
                                                        Bem-vindo ao Management Tickets
                                                    </h1>

                                                    <p style=""
                                                        margin: 0 0 16px 0;
                                                        font-size: 16px;
                                                        line-height: 24px;
                                                        color: #374151;
                                                    "">
                                                        Olá, <strong>{safeName}</strong>!
                                                    </p>

                                                    <p style=""
                                                        margin: 0 0 24px 0;
                                                        font-size: 16px;
                                                        line-height: 24px;
                                                        color: #374151;
                                                    "">
                                                        Sua conta foi criada com sucesso.
                                                    </p>

                                                    <div style=""
                                                        background-color: #f3f4f6;
                                                        border-radius: 8px;
                                                        padding: 16px;
                                                        margin-bottom: 24px;
                                                    "">
                                                        <p style=""
                                                            margin: 0;
                                                            font-size: 14px;
                                                            color: #6b7280;
                                                        "">
                                                            Senha temporária
                                                        </p>

                                                        <p style=""
                                                            margin: 8px 0 0 0;
                                                            font-size: 18px;
                                                            font-weight: 600;
                                                            color: #111827;
                                                        "">
                                                            {safeContent}
                                                        </p>
                                                    </div>

                                                    <p style=""
                                                        margin: 0 0 24px 0;
                                                        font-size: 14px;
                                                        line-height: 20px;
                                                        color: #6b7280;
                                                    "">
                                                        Essa senha deve ser alterada no primeiro acesso.
                                                    </p>

                                                    <!-- BOTÃO DE ACESSO -->

                                                    <table
                                                        width=""100%""
                                                        cellpadding=""0""
                                                        cellspacing=""0""
                                                        border=""0""
                                                        style=""margin: 0 0 24px 0;"">

                                                        <tr>
                                                            <td align=""center"">

                                                                <a
                                                                    href=""https://management-tickets-front-end.vercel.app/login""
                                                                    style=""
                                                                        display: inline-block;
                                                                        background-color: #2563eb;
                                                                        color: #ffffff;
                                                                        text-decoration: none;
                                                                        font-size: 14px;
                                                                        font-weight: 600;
                                                                        padding: 12px 24px;
                                                                        border-radius: 8px;
                                                                    "">
                                                                    Acessar a plataforma
                                                                </a>

                                                            </td>
                                                        </tr>

                                                    </table>

                                                    <!-- LINK ALTERNATIVO -->

                                                    <p style=""
                                                        margin: 0;
                                                        font-size: 13px;
                                                        line-height: 20px;
                                                        color: #6b7280;
                                                        text-align: center;
                                                    "">
                                                        Ou acesse diretamente:
                                                        <br>
                                                        <a
                                                            href=""https://management-tickets-front-end.vercel.app/login""
                                                            style=""
                                                                color: #2563eb;
                                                                text-decoration: none;
                                                            "">
                                                            https://management-tickets-front-end.vercel.app/login
                                                        </a>
                                                    </p>

                                                </td>
                                            </tr>

                                        </table>

                                        <p style=""
                                            margin: 24px 0 0 0;
                                            font-size: 12px;
                                            line-height: 18px;
                                            color: #9ca3af;
                                        "">
                                            Management Tickets
                                        </p>

                                    </td>
                                </tr>

                            </table>

                        </body>
                        </html>
                        ",

                TypeEmailEnum.PasswordRecoveryEmail => $@"
                    <!DOCTYPE html>
                    <html lang=""pt-BR"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Redefinição de senha</title>
                    </head>

                    <body style=""
                        margin: 0;
                        padding: 0;
                        background-color: #f9fafb;
                        font-family: Arial, Helvetica, sans-serif;
                        color: #111827;
                    "">

                        <table
                            width=""100%""
                            cellpadding=""0""
                            cellspacing=""0""
                            border=""0""
                            style=""
                                background-color: #f9fafb;
                                padding: 40px 20px;
                            "">

                            <tr>
                                <td align=""center"">

                                    <table
                                        width=""100%""
                                        cellpadding=""0""
                                        cellspacing=""0""
                                        border=""0""
                                        style=""
                                            max-width: 560px;
                                            background-color: #ffffff;
                                            border: 1px solid #e5e7eb;
                                            border-radius: 12px;
                                        "">

                                        <tr>
                                            <td style=""padding: 40px;"">

                                                <h1 style=""
                                                    margin: 0 0 24px 0;
                                                    font-size: 24px;
                                                    line-height: 32px;
                                                    font-weight: 700;
                                                    color: #111827;
                                                "">
                                                    Redefinição de senha
                                                </h1>

                                                <p style=""
                                                    margin: 0 0 16px 0;
                                                    font-size: 16px;
                                                    line-height: 24px;
                                                    color: #374151;
                                                "">
                                                    Olá, <strong>{safeName}</strong>!
                                                </p>

                                                <p style=""
                                                    margin: 0 0 24px 0;
                                                    font-size: 16px;
                                                    line-height: 24px;
                                                    color: #374151;
                                                "">
                                                    Recebemos uma solicitação para redefinir
                                                    a senha da sua conta no Management Tickets.
                                                </p>

                                                <table
                                                    cellpadding=""0""
                                                    cellspacing=""0""
                                                    border=""0""
                                                    style=""margin: 0 auto 24px auto;"">

                                                    <tr>
                                                        <td
                                                            align=""center""
                                                            style=""
                                                                background-color: #2563eb;
                                                                border-radius: 8px;
                                                            "">

                                                            <a
                                                                href=""{safeContent}""
                                                                style=""
                                                                    display: inline-block;
                                                                    padding: 14px 24px;
                                                                    font-size: 16px;
                                                                    font-weight: 600;
                                                                    color: #ffffff;
                                                                    text-decoration: none;
                                                                "">
                                                                Redefinir minha senha
                                                            </a>

                                                        </td>
                                                    </tr>

                                                </table>

                                                <div style=""
                                                    background-color: #eff6ff;
                                                    border-radius: 8px;
                                                    padding: 16px;
                                                    margin-bottom: 24px;
                                                "">
                                                    <p style=""
                                                        margin: 0;
                                                        font-size: 14px;
                                                        line-height: 20px;
                                                        color: #374151;
                                                    "">
                                                        Este link é válido por
                                                        <strong>30 minutos</strong>
                                                        e pode ser utilizado apenas uma vez.
                                                    </p>
                                                </div>

                                                <p style=""
                                                    margin: 0 0 16px 0;
                                                    font-size: 14px;
                                                    line-height: 20px;
                                                    color: #6b7280;
                                                "">
                                                    Se você não solicitou a redefinição da sua senha,
                                                    pode ignorar este e-mail.
                                                </p>

                                                <p style=""
                                                    margin: 24px 0 0 0;
                                                    padding-top: 24px;
                                                    border-top: 1px solid #e5e7eb;
                                                    font-size: 12px;
                                                    line-height: 18px;
                                                    color: #9ca3af;
                                                "">
                                                    Por segurança, nunca compartilhe este link
                                                    com outras pessoas.
                                                </p>

                                            </td>
                                        </tr>

                                    </table>

                                    <p style=""
                                        margin: 24px 0 0 0;
                                        font-size: 12px;
                                        line-height: 18px;
                                        color: #9ca3af;
                                    "">
                                        Management Tickets
                                    </p>

                                </td>
                            </tr>

                        </table>

                    </body>
                    </html>",

                _ => throw new ArgumentOutOfRangeException(
                    nameof(typeEmail),
                    $"Tipo de email não suportado: {typeEmail}")
            };
        }
    }
}