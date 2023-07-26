namespace Bandit.ACS.Daemon.Services.Contact
{
    public interface ISMSSender
    {
        Task SendSMS(string number, string content);
    }
}
