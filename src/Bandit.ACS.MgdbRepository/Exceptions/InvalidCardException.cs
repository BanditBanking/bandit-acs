namespace Bandit.ACS.MgdbRepository.Exceptions
{
    [Serializable]
    public class InvalidCardException : Exception
    {
        public InvalidCardException() { }

        public InvalidCardException(string message) : base(message) { }
    }
}
