using System.Security.Cryptography.X509Certificates;

namespace Bandit.ACS.Daemon.Services.EidChecking
{
    public interface IEidCheckingAlgorithm
    {
        bool Check(X509Certificate2 certificate, string token, string digest);
    }
}
