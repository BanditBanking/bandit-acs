using System;
using System.Security.Cryptography.X509Certificates;

namespace Bandit.ACS.Daemon.Helpers
{
    public interface ICertificateHelper
    {
        Task<X509Certificate2> LoadCertificateAsync(string fileName);
    }
}
