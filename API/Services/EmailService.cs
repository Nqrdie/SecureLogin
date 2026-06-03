using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using API.Interfaces;
using API.Services.Options;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _options;

        public EmailService(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }

        public async Task<EmailResult> SendEmailAsync(
            string recipientName,
            string recipientEmail,
            string verificationLink)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_options.Name, _options.Email));
                message.To.Add(new MailboxAddress(recipientName, recipientEmail));
                message.Subject = $"Email Verification for {recipientName}";

                var bodyBuilder = new BodyBuilder();

                bodyBuilder.HtmlBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <p>Thanks for signing up {recipientName}!</p>

                    <p>
                    <a href='{verificationLink}'
                        style='
                            display:inline-block;
                            padding:12px 20px;
                            background-color:#4CAF50;
                            color:white;
                            text-decoration:none;
                            border-radius:6px;
                            font-weight:bold;
                        '>
                        Click here to verify
                    </a>
                    </p>
                </body>
                </html>";

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                await client.ConnectAsync(_options.SmtpServer, _options.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_options.Email, _options.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return new EmailResult
                {
                    Success = true,
                    ErrorMessage = null
                };
            }
            catch (Exception ex)
            {
                return new EmailResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}

