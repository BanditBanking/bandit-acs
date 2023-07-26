namespace Bandit.ACS.Daemon.Configuration
{
    public class JWTConfiguration
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifeSpan { get; set; } = 120;
    }
}
