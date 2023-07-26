using Bandit.ACS.NpgsqlRepository;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace Bandit.ACS.Daemon
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            EnsureEfMigration(host);

            await host.RunAsync().ConfigureAwait(false);
        }

        // Due to an EF bug, need to use an host builder: https://stackoverflow.com/questions/55970148/apply-entity-framework-migrations-when-using-asp-net-core-in-a-docker-image
        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder().ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            }).UseStartup<Startup>();

            return builder;
        }

        private static void EnsureEfMigration(IWebHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<NpgsqlDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
