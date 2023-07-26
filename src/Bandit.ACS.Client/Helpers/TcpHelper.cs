using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

namespace Bandit.ACS.Client.Helpers
{
    public static class TcpHelper
    {
        public static void Send<T>(SslStream sslStream, T obj)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            byte[] data = Encoding.UTF8.GetBytes(jsonString);
            sslStream.Write(data, 0, data.Length);
        }

        public static T? Read<T>(SslStream sslStream)
        {
            MemoryStream memoryStream = new MemoryStream();
            sslStream.CopyTo(memoryStream);
            byte[] data = memoryStream.ToArray();
            string jsonString = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
