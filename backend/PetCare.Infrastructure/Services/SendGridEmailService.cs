using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace PetCare.Infrastructure.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly ISendGridClient _client;
        private readonly EmailAddress _from;
        private readonly ILogger<SendGridEmailService> _logger;

        public SendGridEmailService(IConfiguration configuration, ILogger<SendGridEmailService> logger)
        {
            var apiKey = configuration["SendGrid:ApiKey"];
            _client = new SendGridClient(apiKey);
            _from = new EmailAddress(
                configuration["SendGrid:FromEmail"],
                configuration["SendGrid:FromName"]
            );
            _logger = logger;
        }

        public async Task SendAppointmentConfirmationAsync(
            string toEmail, string ownerName, string petName, DateTime appointmentDate,
            CancellationToken cancellationToken = default)
        {
            var subject = $"Confirmación de cita para {petName}";
            var body = $@"
                <p>Hola {ownerName},</p>
                <p>Tu cita para <strong>{petName}</strong> ha sido agendada exitosamente.</p>
                <p><strong>Fecha:</strong> {appointmentDate:dddd, dd 'de' MMMM 'de' yyyy, hh:mm tt}</p>
                <p>Gracias por confiar en PetCare.</p>";

            await SendAsync(toEmail, subject, body, cancellationToken);
        }

        public async Task SendAppointmentReminderAsync(
            string toEmail, string ownerName, string petName, DateTime appointmentDate,
            CancellationToken cancellationToken = default)
        {
            var subject = $"Recordatorio: cita mañana para {petName}";
            var body = $@"
                <p>Hola {ownerName},</p>
                <p>Te recordamos que <strong>{petName}</strong> tiene una cita mañana.</p>
                <p><strong>Fecha:</strong> {appointmentDate:dddd, dd 'de' MMMM 'de' yyyy, hh:mm tt}</p>
                <p>Nos vemos pronto.</p>";

            await SendAsync(toEmail, subject, body, cancellationToken);
        }

        private async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken)
        {
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent: null, htmlContent: htmlBody);

            var response = await _client.SendEmailAsync(msg, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Body.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Error enviando correo a {Email}: {StatusCode} - {Body}",
                    toEmail, response.StatusCode, errorBody);
            }
        }
    }
}