using Bandit.ACS.Daemon.Exceptions;
using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Exceptions
{
    [Serializable]
    public class MaxAttemptsReachedException : Exception, IExposedException
    {
        private readonly ChallengeType _type;
        private readonly int _attempts;

        public MaxAttemptsReachedException() { }

        public MaxAttemptsReachedException(ChallengeType type, int attempts) : base($"Max attempts reached for method {type} (Number of attempts: {attempts})")
        {
            _type = type;
            _attempts = attempts;
        }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status429TooManyRequests,
            ErrorCode = "bunny",
            Title = "Max attempts reached",
            Detail = $"Max attempts reached for method {_type} (Number of attempts: {_attempts})"
        };
    }
}
