using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Mappers
{
    public static class ChallengeExtensions
    {
        public static MgdbRepository.Models.Challenge ToModel(this ChallengeDTO challenge) => new()
        {
            ChallengeId = challenge.Id,
            OTP = challenge.Code
        };

        public static ChallengeDTO ToContract(this MgdbRepository.Models.Challenge challenge) => new()
        {
            Id = challenge.ChallengeId,
            Code = challenge.OTP
        };
    }
}
