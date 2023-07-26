using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Exceptions
{
    [Serializable]
    public class AccountNotFoundException : Exception, IExposedException
    {
        private Guid _accountId;

        public AccountNotFoundException() { }

        public AccountNotFoundException(Guid accountId) : base($"Account with id {accountId} could not be found") 
        {
            _accountId = accountId;
        }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status404NotFound,
            ErrorCode = "flash",
            Title = "Account not found",
            Detail = $"An account with id {_accountId} could not be found"
        };
    }
}
