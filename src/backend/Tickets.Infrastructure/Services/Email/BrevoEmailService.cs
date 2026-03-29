using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Tickets.Application.Interfaces;
using Tickets.Exceptions.ExceptionBase;
using Tickets.Infrastructure.Settings;

namespace Tickets.Infrastructure.Services.Email
{
    public class BrevoEmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly BrevoSettings _settings;

        public BrevoEmailService(HttpClient httpClient, IOptions<BrevoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;

            _httpClient.BaseAddress = new Uri("https://api.brevo.com/v3/");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("api-key", _settings.ApiKey);
        }

        public async Task SendAsync(string toEmail, string toName, string subject, string htmlContent)
        {
            var payload = new
            {
                sender = new
                {
                    email = _settings.SenderEmail,
                    name = _settings.SenderName
                },
                to = new[]
                {
                    new
                    {
                        email = toEmail,
                        name = toName
                    }
                },
                subject,
                htmlContent
            };

            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("smtp/email", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new BusinessRuleException($"Error sending email with Brevo: {error}");
            }
        }
    }
}
