using Bandit.ACS.MgdbRepository.Repositories;
using Bandit.ACS.MgdbRepository.Utils;
using Bandit.ACS.Configuration;
using Bandit.ACS.Daemon.Extensions;
using Bandit.ACS.Daemon.Helpers;
using Bandit.ACS.Daemon.Services;
using Microsoft.EntityFrameworkCore;
using Bandit.ACS.NpgsqlRepository;
using Bandit.ACS.NpgsqlRepository.Repositories;
using Bandit.ACS.Daemon.CommandHandlers;
using Bandit.ACS.Daemon.Services.EidChecking;
using Bandit.ACS.Daemon.Services.Contact;

namespace Bandit.ACS.Daemon
{
    public class Startup
    {
        private IConfiguration _configuration;
        private DaemonConfiguration _parsedConfiguration;
        private IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _parsedConfiguration = configuration.GetSection(DaemonConfiguration.ServiceName).Get<DaemonConfiguration>() ?? new DaemonConfiguration();
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddSingleton(_parsedConfiguration)
                .AddEndpointsApiExplorer()
                .AddCorsHandling()
                .AddJwtAuthentication(_parsedConfiguration.JWT)
                .AddSwaggerService(_parsedConfiguration.API)
                .AddLogging(b =>
                {
                    b.AddConfiguration(_configuration.GetSection("Logging"));
                    b.AddConsole();
                })
                .AddCommandHandling(b =>
                {
                    b.AddHandler<PaymentRequestHandler>(PaymentRequestHandler.Predicate);
                })
                .AddSingleton<ICertificateHelper, LocalCertificateHelper>()
                .AddSingleton(provider => MgdbDatabaseFactory.Create(_parsedConfiguration.MgdbDatabase.ConnectionString, _parsedConfiguration.MgdbDatabase.DatabaseName))
                .AddScoped<ICommandParser, CommandParser>()
                .AddScoped<ITransactionRepository, TransactionRepository>()
                .AddScoped<IAccountsRepository, AccountRepository>()
                .AddScoped<ICardsRepository, CardsRepository>()
                .AddScoped<IChallengeRepository, ChallengeRepository>()
                .AddScoped<ITokenService, JwtTokenService>()
                .AddScoped<ITransactionService, TransactionService>()
                .AddScoped<IAccountsService, AccountsService>()
                .AddScoped<ICardsService, CardsService>()
                .AddScoped<IChallengeService, ChallengeService>()
                .AddScoped<IAnalyticsService, AnalyticsService>()
                .AddScoped<ICertificateChecker, BelgiumCitizenRootCAChecker>()
                .AddScoped<EidCheckingFactory>()
                .AddScoped<ISMSSender, TwilioSMSSender>()
                .AddScoped<IMailSender, MailSender>()
                .AddHostedService<TCPService>()
                .AddHostedService<SyncService>();

            if (_environment?.IsEnvironment("Testing") ?? false)
            {
                services.AddDbContext<NpgsqlDbContext>(options => options.UseInMemoryDatabase("bandit-npgsql-db-test"));
            } else
            {
                services.AddDbContext<NpgsqlDbContext>(options => options.UseNpgsql(_parsedConfiguration.NpgsqlDatabase.ConnectionString));
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("ALLOW_ANY");
            app.UseExceptionHandler(options =>
            {
                options.DocumentationPath = _parsedConfiguration.API.ErrorDocumentationUri;
            });

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
