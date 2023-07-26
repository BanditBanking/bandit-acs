using Bandit.ACS.Daemon.Configuration;

namespace Bandit.ACS.Configuration
{
    public class DaemonConfiguration
    {
        public const string ServiceName = "ACS";
        public string BankIdentifier { get; set; }
        public DatabaseConfiguration MgdbDatabase { get; set; }
        public DatabaseConfiguration NpgsqlDatabase { get; set; }
        public TCPConfiguration TCP { get; set; }
        public SSLConfiguration SSL { get; set; }
        public JWTConfiguration JWT { get; set; }
        public APIConfiguration API { get; set; }
        public AnalyticsConfiguration Analytics { get; set; }
        public TwilioConfiguration Twilio { get; set; }
        public MailConfiguration Mail { get; set; }
    }
}
