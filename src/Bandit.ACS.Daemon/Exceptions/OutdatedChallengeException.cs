using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Exceptions
{
    public class OutdatedChallengeException : Exception, IExposedException
    {
        public OutdatedChallengeException() { }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status410Gone,
            ErrorCode = "parrot",
            Title = "The challenge is outdated",
            Detail = $"This challenge cannot be verified anymore"
        };
    }
}

