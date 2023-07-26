using Bandit.ACS.Daemon.Exceptions;
using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Exceptions
{
    [Serializable]
    public class ChallengeAlreadyCompletedException : Exception, IExposedException
    {
        private Guid _challengeId;

        public ChallengeAlreadyCompletedException() { }

        public ChallengeAlreadyCompletedException(Guid challengeId) : base($"Challenge with id {challengeId} already completed") 
        {
            _challengeId = challengeId;
        }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status410Gone,
            ErrorCode = "bearer",
            Title = "Challenge already completed",
            Detail = $"Challenge id {_challengeId} already completed"
        };
    }
}
