namespace Bandit.ACS.Daemon.Configuration
{
    public class AnalyticsConfiguration
    {
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public string ServerCertificate { get; set; }
        public int SyncPeriodInMinutes { get; set; }
    }
}
