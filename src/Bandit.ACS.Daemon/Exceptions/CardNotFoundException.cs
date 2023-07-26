using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Exceptions
{
    [Serializable]
    public class CardNotFoundException : Exception, IExposedException
    {
        private string _cardNumber;

        public CardNotFoundException() { }

        public CardNotFoundException(string cardNumber) : base($"Card {cardNumber} could not be found") 
        {
            _cardNumber = cardNumber;
        }

        public ProblemDetailDTO Expose() => new()
        {
            HttpCode = StatusCodes.Status404NotFound,
            ErrorCode = "crowbar",
            Title = "Card not found",
            Detail = $"Card {_cardNumber} could not be found"
        };
    }
}
