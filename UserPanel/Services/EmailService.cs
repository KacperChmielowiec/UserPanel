using Microsoft.Extensions.Options;
using MimeKit;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Config;
using MailKit.Net.Smtp;
namespace UserPanel.Services
{
    public class EmailService : IEmailSender
    {
        private EmailConfiguration _configuration;

        public EmailService(IOptions<EmailConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }
        public void SendEmail(Email email)
        {
            MimeMessage mimeMessage = CreateEmail(email);
            Send(mimeMessage);
        }

        private MimeMessage CreateEmail(Email email)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("email", _configuration.from));
            message.To.AddRange(email.To.ToArray());
            message.Subject = email.Subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = email.Body};

            return message;
        }

        private void Send(MimeMessage message)
        {
            using var clientSMTP = new SmtpClient();
            try
            {
                clientSMTP.CheckCertificateRevocation = false;
                clientSMTP.Connect(_configuration.server,_configuration.port,true);
                clientSMTP.AuthenticationMechanisms.Remove("XOAUTH2");
                clientSMTP.Authenticate(_configuration.userName,_configuration.password);

                clientSMTP.Send(message);

            }catch (Exception ex)
            {
                throw;
            }
            finally
            {
                clientSMTP.Disconnect(true);
                clientSMTP.Dispose();
            }
        }
    }
}
