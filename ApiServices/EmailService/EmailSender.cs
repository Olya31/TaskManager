using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace ApiServices.EmailService
{
    public sealed class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = string.Format("<h2 style='color:black;'>{0}</h2>", message.Content),
            };

            if (message.Attachment != null)
            {
                byte[] fileBytes;

                using (var ms = new MemoryStream())
                {
                    message.Attachment.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                bodyBuilder.Attachments.Add(
                    message.Attachment.FileName,
                    fileBytes);
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                await client.SendAsync(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
