namespace Bandit.ACS.Daemon.Exceptions
{
    [Serializable]
    public class InvalidPaymentTokenException : Exception
    {

        public InvalidPaymentTokenException() { }

        public InvalidPaymentTokenException(string paymentToken) : base($"Payment token \"{paymentToken}\" invalid")
        {
        }
    }
}
