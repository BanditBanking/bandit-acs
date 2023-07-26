using Bandit.ACS.Daemon.Exceptions;
using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Exceptions
{
    [Serializable]
    public class InvalidChallengeIdException : Exception, IExposedException
    {
        private Guid _challengeId;

        public InvalidChallengeIdException() { }

        public InvalidChallengeIdException(Guid challengeId) : base($"Challenge with id {challengeId} not found") 
        {
            _challengeId = challengeId;
        }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status404NotFound,
            ErrorCode = "jellyfish",
            Title = "Challenge not found",
            Detail = $"Challenge id {_challengeId} not found"
        };
    }
}
