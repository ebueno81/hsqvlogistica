using HsqvLogistica.Services.Interfaces;
using MimeKit;

namespace HsqvLogistica.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfigurationService _configurationService;

        public EmailService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task SendAsync(
            IEnumerable<string> destinatarios,
            string asunto,
            string html)
        {
            var smtp = await _configurationService.GetSmtpSettingsAsync();

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("HSQV Logística", smtp.From));

            foreach (var correo in destinatarios)
            {
                message.To.Add(MailboxAddress.Parse(correo));
            }

            message.Subject = asunto;

            message.Body = new BodyBuilder
            {
                HtmlBody = html
            }.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            await client.ConnectAsync(
                smtp.Host,
                smtp.Port,
                smtp.EnableSsl);

            await client.AuthenticateAsync(
                smtp.Username,
                smtp.Password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
