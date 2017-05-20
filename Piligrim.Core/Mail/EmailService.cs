using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Piligrim.Core.Mail
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<MailConfiguration> configuration;

        public EmailService(IOptions<MailConfiguration> configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var conf = this.configuration.Value;
            var emailMessage = new MimeMessage();


            emailMessage.From.Add(new MailboxAddress(conf.From.Name, conf.From.Address));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(conf.Smtp.Host, conf.Smtp.Port, conf.Smtp.UseSsl);
                await client.AuthenticateAsync(conf.Smtp.Login, conf.Smtp.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}