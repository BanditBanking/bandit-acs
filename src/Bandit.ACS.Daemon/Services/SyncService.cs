using Bandit.ACS.Configuration;

namespace Bandit.ACS.Daemon.Services
{
    public class SyncService : BackgroundService
    {
        private readonly ILogger<SyncService> _logger;
        private readonly TimeSpan _period;
        private readonly IAnalyticsService _analyticsService;

        public SyncService(ILogger<SyncService> logger, IAnalyticsService analyticsService, DaemonConfiguration config)
        {
            _logger = logger;
            _analyticsService = analyticsService;
            _period = TimeSpan.FromMinutes(config.Analytics.SyncPeriodInMinutes);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (!cancellationToken.IsCancellationRequested && await timer.WaitForNextTickAsync(cancellationToken))
            {
                try
                {
                    await _analyticsService.SynchronizeChallengesAsync();
                    _logger.LogInformation("Successfully synchronized challenges with the analytics server");
                }
                catch (Exception)
                {
                    _logger.LogError("An error occured while trrying to sync challenges with the analytics server");
                }
            }
        }
    }
}
