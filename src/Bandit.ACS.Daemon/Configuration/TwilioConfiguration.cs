namespace Bandit.ACS.Daemon.Configuration
{
    public record TwilioConfiguration
    {
        public string AccountSid { get; set; }
        public string MessagingServiceSid { get; set; }
        public string AuthToken { get; set; }
    }
}
