namespace Bandit.ACS.Daemon.Services.Contact
{
    public interface IMailSender
    {
        Task SendMail(string to, string subject, string content);
    }
}
