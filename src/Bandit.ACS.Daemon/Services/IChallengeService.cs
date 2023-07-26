using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Services
{
    public interface IChallengeService
    {
        Task<AttemptResult> AttemptAsync(ChallengeAttemptDTO challenge, Account challenger, string ip);
        Task<ChallengeDTO> GenerateChallengeAsync(Account challenger, string ip, ChallengeType type);
        Task AttemptEIDAsync(EidChallengeAttemptDTO challenge, string ip);
        Task<AttemptResult> VerifyEIDChallenge(Guid token);
        Task<AttemptResult> AttemptSentCode(ChallengeAttemptDTO challenge, Account challenger, string ip, ChallengeType type);
    }
}
