using Card = Bandit.ACS.Daemon.Models.Card;

namespace Bandit.ACS.Daemon.Mappers
{
    public static class CardExtensions
    {
        public static MgdbRepository.Models.Card ToModel(this Card card) => new()
        {
            CardNumber = card.CardNumber,
            PinCode = card.PinCode,
            OwnerId = card.OwnerId
        };

        public static Card ToContract(this MgdbRepository.Models.Card card) => new()
        {
            OwnerId = card.OwnerId,
            CardNumber = card.CardNumber,
            PinCode = card.PinCode,
            Balance = card.Balance
        };
    }
}
