using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Bandit.ACS.Helpers
{
    public class SSLCertificateHelper
    {
        public static X509Certificate2 LoadCertificate(string storeName, string storeLocation, string commonName)
        {
            X509Store store = new X509Store(Enum.Parse<StoreName>(storeName), Enum.Parse<StoreLocation>(storeLocation));
            store.Open(OpenFlags.ReadOnly);
            return store.Certificates.Find(X509FindType.FindBySubjectName, commonName, true).First();
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("[SSL] Certificate error: {0}", sslPolicyErrors);

            return false;
        }
    }
}
