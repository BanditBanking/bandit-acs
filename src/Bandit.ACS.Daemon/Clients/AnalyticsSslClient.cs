using Bandit.ACS.Commands;
using Bandit.ACS.Daemon.Exceptions;
using Bandit.ACS.Daemon.Helpers;
using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Bandit.ACS.Client
{
    public class AnalyticsSslClient : IDisposable
    {
        private readonly SslStream _sslStream;
        private readonly TcpClient _client;
        private readonly X509Certificate2 _serverCertificate;

        public AnalyticsSslClient(string host, int port, X509Certificate2 serverCertificate)
        {
            try
            {
                _client = new TcpClient(host, port);
                _serverCertificate = serverCertificate;
                _sslStream = new SslStream(_client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                _sslStream.AuthenticateAsClient(host);
            } catch(Exception ex)
            {
                throw new AnalyticsServerUnreachableException(host, port);
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            // TODO: Verify that the certificate match the expected server certificate
            // return sslPolicyErrors == SslPolicyErrors.None;
            return true;
        }

        public async Task<ChallengeAnalyticsResultDTO> SyncChallengesAsync(List<AnalyticsChallenge> challenges)
        {
            var command = new ChallengeAnalyticsCommand { Challenges = challenges };
            await TcpHelper.SendAsync(_sslStream, command);
            return await TcpHelper.ReadAsync<ChallengeAnalyticsResultDTO>(_sslStream);
        }

        public void Dispose()
        {
            _sslStream.Dispose();
            _client.Dispose();
        }
    }
}
