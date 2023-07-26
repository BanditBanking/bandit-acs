using System.Net.Security;
using System.Text;
using System.Text.Json;

namespace Bandit.ACS.Daemon.Helpers
{
    public static class TcpHelper
    {
        public static async Task SendAsync<T>(SslStream sslStream, T obj)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            byte[] data = Encoding.UTF8.GetBytes(jsonString);
            await sslStream.WriteAsync(data, 0, data.Length);
            await sslStream.FlushAsync();
        }

        public static async Task<T> ReadAsync<T>(SslStream sslStream)
        {
            MemoryStream memoryStream = new MemoryStream();
            await sslStream.CopyToAsync(memoryStream);
            byte[] data = memoryStream.ToArray();
            string jsonString = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
