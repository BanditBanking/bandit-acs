namespace Bandit.ACS.Configuration
{
    public class SSLConfiguration
    {
        public string StorePath { get; set; } = "/certs";

        public string ServerCertificate { get; set; } = "default.crt";

    }
}
