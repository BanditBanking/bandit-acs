using Bandit.ACS.MgdbRepository.Models;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Bandit.ACS.Daemon.Services.EidChecking
{
    public class RSASha256EIDAlgorithm : IEidCheckingAlgorithm
    {
        public bool Check(X509Certificate2 certificate, string token, string digest)
        {
            var rsa = certificate.GetRSAPublicKey();
            return rsa.VerifyData(Encoding.UTF8.GetBytes(token), Convert.FromBase64String(digest), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
