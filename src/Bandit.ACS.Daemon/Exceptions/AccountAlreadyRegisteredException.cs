using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Exceptions
{
    [Serializable]
    public class AccountAlreadyRegisteredException : Exception, IExposedException
    {
        private string _mail;

        public AccountAlreadyRegisteredException() { }

        public AccountAlreadyRegisteredException(string mail) : base($"Account with email {mail} already registered") 
        {
            _mail = mail;
        }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status409Conflict,
            ErrorCode = "glowfish",
            Title = "Account already exists",
            Detail = $"An account with email {_mail} is already registered"
        };
    }
}
