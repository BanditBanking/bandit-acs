using Bandit.ACS.Client.Helpers;
using Bandit.ACS.Commands;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Bandit.ACS.Client
{
    public class AcsTcpClient : IDisposable
    {
        private readonly SslStream _sslStream;
        private readonly TcpClient _client;

        public AcsTcpClient(string host, int port)
        {
            _client = new TcpClient(host, port);
            _sslStream = new SslStream(_client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            _sslStream.AuthenticateAsClient(host);
        }

        public void CodeValidation(string code)
        {
            var command = new CodeValidationCommand { Code = code };
            TcpHelper.Send(_sslStream, command);
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            // return sslPolicyErrors == SslPolicyErrors.None;
            return true;
        }

        public void Dispose()
        {
            _sslStream.Dispose();
            _client.Dispose();
        }
    }
}
