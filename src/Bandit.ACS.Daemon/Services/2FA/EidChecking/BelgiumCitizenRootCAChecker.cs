using Bandit.ACS.Configuration;
using Bandit.ACS.Daemon.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace Bandit.ACS.Daemon.Services.EidChecking
{
    public class BelgiumCitizenRootCAChecker : ICertificateChecker
    {
        private readonly SSLConfiguration _configuration;
        public BelgiumCitizenRootCAChecker(DaemonConfiguration configuration)
        {
            _configuration = configuration.SSL;
        }

        public bool CheckCertificateAuthenticity(X509Certificate2 certificate)
        {
            var authority = GetBelgiumRootCA();
            var chain = GetChain(authority);
            var isChainValid = chain.Build(certificate);

            if (!isChainValid)
                return false;

            var valid = chain.
                ChainElements.
                Cast<X509ChainElement>().
                Any(x => x.Certificate.Thumbprint == authority.Thumbprint);

            return isChainValid && valid;
        }

        private X509Certificate2 GetBelgiumRootCA()
        {
            var beCertificate = $"{_configuration.StorePath}/belgiumrca4.crt";
            var bytes = File.ReadAllBytes(beCertificate);
            var authority = new X509Certificate2(bytes);

            return authority;
        }

        private X509Chain GetChain(X509Certificate2 authority)
        {
            X509Chain chain = new X509Chain();
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
            chain.ChainPolicy.VerificationTime = DateTime.Now;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 0);
            chain.ChainPolicy.ExtraStore.Add(authority);

            return chain;
        }
    }
}
