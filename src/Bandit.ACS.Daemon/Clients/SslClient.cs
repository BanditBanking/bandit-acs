using Bandit.ACS.Helpers;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;

namespace Bandit.ACS.Clients
{
    public class SslClient : IDisposable
    {
        private readonly string _host;
        private readonly int _port;
        private SslStream _sslStream;
        private TcpClient _client;

        public SslClient(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public SslClient(SslStream sslStream)
        {
            _sslStream = sslStream;
        }

        public void Connect()
        {
            _client = new TcpClient(_host, _port);
            _sslStream = new SslStream(_client.GetStream(), false, new RemoteCertificateValidationCallback(SSLCertificateHelper.ValidateServerCertificate), null);
            try
            {
                _sslStream.AuthenticateAsClient(_host);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Authentication failed - closing the connection.");
                _client.Close();
            }
        }

        public async Task SendAsync<T>(T obj)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            byte[] data = Encoding.UTF8.GetBytes(jsonString);
            await _sslStream.WriteAsync(data, 0, data.Length);
            await _sslStream.FlushAsync();
        }

        public async Task<string> ReadStringAsync()
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[4096];
            int bytesRead;

            do
            {
                bytesRead = await _sslStream.ReadAsync(buffer, 0, buffer.Length);
                ms.Write(buffer, 0, bytesRead);
            } while (bytesRead == buffer.Length);

            string message = Encoding.UTF8.GetString(ms.ToArray());
            return message;
        }

        public async Task<T> ReadObjectAsync<T>()
        {
            MemoryStream memoryStream = new MemoryStream();
            await _sslStream.CopyToAsync(memoryStream);
            byte[] data = memoryStream.ToArray();
            string jsonString = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public void Dispose()
        {
            _sslStream?.Dispose();
            _client?.Dispose();
        }
    }
}
