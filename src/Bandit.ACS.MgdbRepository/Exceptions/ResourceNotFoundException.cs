namespace Bandit.ACS.MgdbRepository.Exceptions
{
    [Serializable]
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() { }

        public ResourceNotFoundException(string message) : base(message) { }
    }
}
