using MimeKit;
using MimeKit.Text;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Bandit.ACS.Configuration;

namespace Bandit.ACS.Daemon.Services.Contact
{
    public class MailSender : IMailSender
    {
        private readonly EmailData _data;

        public MailSender(DaemonConfiguration configuration)
        {
            _data = new EmailData
            {
                From = configuration.Mail.From,
                Password = configuration.Mail.Password,
                Username = configuration.Mail.From,
                Port = configuration.Mail.Port,
                Smtp = configuration.Mail.Smtp
            };
        }

        public async Task SendMail(string to, string subject, string content)
        {
            MimeMessage email;

            if (!string.IsNullOrEmpty(_data.ReplyToEmail) && !string.IsNullOrEmpty(_data.ReplyToFirstName) && !string.IsNullOrEmpty(_data.ReplyToLastName))
            {
                email = new MimeMessage()
                {
                    Subject = subject,
                    Body = new TextPart(TextFormat.Html) { Text = $"{_data.ReplyToFirstName} {_data.ReplyToLastName} ({_data.ReplyToEmail}):<br><br>{content}" }
                };
                email.ReplyTo.Add(new MailboxAddress($"{_data.ReplyToFirstName} {_data.ReplyToLastName}", _data.ReplyToEmail));
            }
            else
            {
                email = new MimeMessage()
                {
                    Subject = subject,
                    Body = new TextPart(TextFormat.Html) { Text = content }
                };
            }

            email.From.Add(MailboxAddress.Parse(_data.From));
            email.To.Add(MailboxAddress.Parse(to));

            using var smtp = new SmtpClient();
            smtp.Connect(_data.Smtp, _data.Port);
            smtp.Authenticate(_data.Username, _data.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }

}
