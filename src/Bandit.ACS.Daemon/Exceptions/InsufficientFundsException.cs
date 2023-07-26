using Bandit.ACS.Daemon.Exceptions;
using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Exceptions
{
    [Serializable]
    public class InsufficientFundsException : Exception, IExposedException
    {
        private readonly double _requestedAmount;

        public InsufficientFundsException() { }

        public InsufficientFundsException(double requestedAmount) : base($"Insufficient balance to proceed a transaction of {requestedAmount} €")
        {
            _requestedAmount = requestedAmount;
        }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status402PaymentRequired,
            ErrorCode = "penniless",
            Title = "Insufficient funds",
            Detail = $"Insufficient balance to proceed a transaction of {_requestedAmount} €"
        };
    }
}
