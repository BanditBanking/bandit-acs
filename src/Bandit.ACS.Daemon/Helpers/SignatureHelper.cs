using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Bandit.ACS.Helpers
{
    public class SignatureHelper
    { 

        public static string Sign(string authenticationCode, string certificateName)
        {
            X509Certificate2 serverCertificate = SSLCertificateHelper.LoadCertificate("My", "LocalMachine", certificateName);
            using (var rsa = serverCertificate.GetRSAPrivateKey())
            {
                var hash = SHA1Hash(authenticationCode);
                return Convert.ToBase64String(rsa.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1));
            }
        }

        public static bool Verify(string data, string signature, string certificateName)
        {
            X509Certificate2 clientCertificate = SSLCertificateHelper.LoadCertificate("My", "LocalMachine", certificateName);
            var dataHash = SHA1Hash(data);
            using (var rsa = clientCertificate.GetRSAPublicKey())
            {
                var signatureBytes = Convert.FromBase64String(signature);
                var isValid = rsa.VerifyHash(dataHash, signatureBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                return isValid;
            }
        }

        private static byte[] SHA1Hash(string data)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var digest = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
                var b64Digest = System.Convert.ToBase64String(digest);
                return Convert.FromBase64String(b64Digest);
            }
        }
    }
}