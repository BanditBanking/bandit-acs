using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Services
{
    public interface IAnalyticsService
    {
        Task<ChallengeAnalyticsResultDTO> SynchronizeChallengesAsync();
    }
}
