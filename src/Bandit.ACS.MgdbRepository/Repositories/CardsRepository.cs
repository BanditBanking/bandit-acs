using Bandit.ACS.MgdbRepository.Exceptions;
using Bandit.ACS.MgdbRepository.Models;
using MongoDB.Driver;

namespace Bandit.ACS.MgdbRepository.Repositories
{
    public class CardsRepository : ICardsRepository
    {
        private readonly IMongoCollection<Card> _cards;

        public CardsRepository(IMongoDatabase database)
        {
            _cards = database.GetCollection<Card>("cards");
        }

        public async Task AddCard(Card card) => await _cards.InsertOneAsync(card);

        public async Task<bool> ExistsAsync(string cardNumber) => await _cards.Find(c => c.CardNumber == cardNumber).AnyAsync();

        public async Task<Card> GetByCardNumberAsync(string cardNumber)
        {
            var card = await _cards.Find(a => a.CardNumber == cardNumber).FirstOrDefaultAsync();

            if(card == null)
                throw new ResourceNotFoundException($"A card with number {cardNumber} could not be found");

            return card;
        }

        public async Task<Card> GetCardByOwnerAsync(Guid ownerId) => await _cards.Find(c => c.OwnerId == ownerId).FirstOrDefaultAsync();
        public async Task<List<Card>> GetOwnerCardsAsync(Guid ownerId) => await _cards.Find(c => c.OwnerId == ownerId).ToListAsync();

        public async Task SetBalanceAsync(string cardNumber, double balance)
        {
            var card = await GetByCardNumberAsync(cardNumber);
            card.Balance = balance;
            await _cards.ReplaceOneAsync((a) => a.CardNumber == cardNumber, card);
        }
    }
}
