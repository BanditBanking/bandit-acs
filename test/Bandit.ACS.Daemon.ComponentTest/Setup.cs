using Bandit.ACS.Client;
using Bandit.ACS.Configuration;
using Bandit.ACS.Daemon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandit.ACS.ComponentTest
{
    [TestClass]
    public class Setup
    {

        private static TestServer _server;

        public IServiceProvider ServiceProvider => _server.Host.Services;

        public static AcsTcpClient AcsTcpClient { get; private set; }


        [AssemblyInitialize]
        public static Task AssemblyInitializeAsync(TestContext _)
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, false).Build();
            var config = configBuilder.GetSection(DaemonConfiguration.ServiceName).Get<DaemonConfiguration>() ?? new DaemonConfiguration();

            _server = new TestServer(
                new WebHostBuilder()
                    .UseEnvironment("Testing")
                    .UseStartup<Startup>()
                    .ConfigureAppConfiguration(b => b
                        .AddJsonFile("appsettings.json", false, false)));

            AcsTcpClient = new AcsTcpClient("localhost", 6001);

            return Task.CompletedTask;
        }
    }
}
