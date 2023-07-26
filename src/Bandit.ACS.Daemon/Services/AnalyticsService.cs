using Bandit.ACS.Client;
using Bandit.ACS.Configuration;
using Bandit.ACS.Daemon.Helpers;
using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.MgdbRepository.Repositories;

namespace Bandit.ACS.Daemon.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ICertificateHelper _certificateHelper;
        private readonly IChallengeRepository _challengeRepository;
        private readonly DaemonConfiguration _config;

        public AnalyticsService(ICertificateHelper certificateHelper, DaemonConfiguration config, IChallengeRepository challengeRepository)
        {
            _certificateHelper = certificateHelper;
            _challengeRepository = challengeRepository;
            _config = config;
        }

        public async Task<ChallengeAnalyticsResultDTO> SynchronizeChallengesAsync()
        {
            var serverCertificate = await _certificateHelper.LoadCertificateAsync(_config.Analytics.ServerCertificate);
            using var analyticsClient = new AnalyticsSslClient(_config.Analytics.ServerAddress, _config.Analytics.ServerPort, serverCertificate);
            var challenges = await _challengeRepository.GetAfterDateAsync(DateTime.Today.AddDays(-1));
            var analyticsChallenges = challenges.Select(c => new AnalyticsChallenge
            {
                ChallengeId = c.ChallengeId,
                ChallengeType = c.ChallengeType.ToString(),
                BankId = c.BankId,
                ClientId = c.ChallengerId,
                BirtDate = c.ChallengerBirthDay,
                Age = c.ChallengerAge,
                Gender = c.ChallengerGender,
                RequestTime = c.RequestTime,
                AttemptCount = 3 - c.RemainingAttempts,
                ResponseTime = c.ResponseTime,
                Decision = c.IsSuccess ? "Accepted" : "Refused",
                MaxAttemptsReached = c.RemainingAttempts <= 0,
                DecisionTime = c.DecisionTime
            }).ToList();
            return await analyticsClient.SyncChallengesAsync(analyticsChallenges);
        }
    }
}
