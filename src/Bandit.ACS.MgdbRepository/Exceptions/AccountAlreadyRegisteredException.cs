namespace Bandit.ACS.MgdbRepository.Exceptions
{
    [Serializable]
    public class AccountAlreadyRegisteredException : Exception
    {
        public AccountAlreadyRegisteredException() { }

        public AccountAlreadyRegisteredException(string message) : base(message) { }
    }
}
