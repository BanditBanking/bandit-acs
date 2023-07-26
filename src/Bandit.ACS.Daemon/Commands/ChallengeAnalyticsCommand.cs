using Bandit.ACS.Daemon.Models;

namespace Bandit.ACS.Commands
{
    public class ChallengeAnalyticsCommand : ICommand
    {
        public string Type { get; set; } = nameof(ChallengeAnalyticsCommand);
        public List<AnalyticsChallenge> Challenges { get; set; }
    }
}
