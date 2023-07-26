using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Services
{
    public interface ICardsService
    {
        Task AddCardAsync(Card card);
        Task<bool> EnsureFundsAsync(CardCredentialsDTO card, double amount);
        Task<Card> GenerateCardAsync(Guid ownerId);
        Task<List<Card>> GetOwnerCardsAsync(Guid ownerId);
        Task SetBalanceAsync(string cardNumber, double balance);
    }
}
