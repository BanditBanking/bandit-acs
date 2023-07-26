using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Exceptions
{
    public class InvalidAlgorithmException : Exception, IExposedException
    {
        public InvalidAlgorithmException() { }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status422UnprocessableEntity,
            ErrorCode = "sharknado",
            Title = "Unrecognized algorithm",
            Detail = $"The algorithm couldn't be processed"
        };
    }
}