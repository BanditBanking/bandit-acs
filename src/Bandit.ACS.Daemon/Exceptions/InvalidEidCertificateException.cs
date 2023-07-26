using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Exceptions
{
    public class InvalidEidCertificateException : Exception, IExposedException
    {
        public InvalidEidCertificateException() { }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status412PreconditionFailed,
            ErrorCode = "dachshund",
            Title = "Invalid belgian certificate",
            Detail = $"This certificate was not verified by the Belgium Root CA"
        };
    }
}
