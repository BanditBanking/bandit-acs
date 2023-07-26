namespace Bandit.ACS.Daemon.Configuration
{
    public record MailConfiguration
    {
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
    }
}
