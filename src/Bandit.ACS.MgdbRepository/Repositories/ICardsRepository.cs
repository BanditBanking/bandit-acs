using Bandit.ACS.MgdbRepository.Models;

namespace Bandit.ACS.MgdbRepository.Repositories
{
    public interface ICardsRepository
    {
        Task AddCard(Card card);
        Task<bool> ExistsAsync(string cardNumber);
        Task<Card> GetByCardNumberAsync(string cardNumber);
        Task<Card> GetCardByOwnerAsync(Guid id);
        Task<List<Card>> GetOwnerCardsAsync(Guid ownerId);
        Task SetBalanceAsync(string cardNumber, double balance);
    }
}
