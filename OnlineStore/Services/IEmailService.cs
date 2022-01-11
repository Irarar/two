using System;
using System.Threading.Tasks;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace OnlineStore.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message);
    }
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "dudoser255423167@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("dudoser255423167@mail.ru", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 25, false);
                await client.AuthenticateAsync("dudoser255423167@mail.ru", "8wjsgZd1rTeguxNatqFS");
                try
                {
                    await client.SendAsync(emailMessage);
                }
                catch (Exception e)
                {

                }
                await client.DisconnectAsync(true);
            }
        }
    }
}