using System.Security.Cryptography.X509Certificates;

namespace Bandit.ACS.Daemon.Services.EidChecking
{
    public interface ICertificateChecker
    {
        bool CheckCertificateAuthenticity(X509Certificate2 certificate);
    }
}
