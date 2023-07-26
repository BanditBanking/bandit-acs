using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Bandit.ACS.Daemon.Services.EidChecking
{
    public class ECSha384EIDAlgorithm : IEidCheckingAlgorithm
    {
        public bool Check(X509Certificate2 certificate, string token, string digest)
        {
            var ec = certificate.GetECDsaPublicKey();
            return ec.VerifyData(Encoding.UTF8.GetBytes(token), Convert.FromBase64String(digest), HashAlgorithmName.SHA384);
        }
    }
}
