using Bandit.ACS.MgdbRepository.Repositories;
using Bandit.ACS.Daemon.Mappers;
using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.MgdbRepository.Exceptions;
using Bandit.ACS.Daemon.Exceptions;

namespace Bandit.ACS.Daemon.Services
{
    public class CardsService : ICardsService
    {
        private readonly ICardsRepository _cardsRepository;
        private readonly IAccountsRepository _accountsRepository;

        public CardsService(ICardsRepository cardsRepository, IAccountsRepository accountsRepository)
        {
            _cardsRepository = cardsRepository;
            _accountsRepository = accountsRepository;
        }

        public async Task<List<Card>> GetOwnerCardsAsync(Guid ownerId)
        {
            try
            {
                var cards = await _cardsRepository.GetOwnerCardsAsync(ownerId);
                return cards.Select(c => c.ToContract()).ToList();
            }
            catch (ResourceNotFoundException)
            {
                throw new AccountNotFoundException(ownerId);
            }
        }

        public async Task AddCardAsync(Card card)
        {
            await _cardsRepository.AddCard(card.ToModel());
        }

        public async Task<Card> GenerateCardAsync(Guid ownerId)
        {
            if (!await _accountsRepository.ExistsAsync(ownerId)) throw new AccountNotFoundException(ownerId);

            Card card = new()
            {
                OwnerId = ownerId,
                CardNumber = GenerateRandomString(16),
                PinCode = GenerateRandomNumber(4),
            };

            await _cardsRepository.AddCard(card.ToModel());

            return card;

        }

        private string GenerateRandomString(int digitsCount)
        {
            var random = new Random();
            string randomNumberAsString = "";
            for (int i = 0; i < digitsCount; i++)
            {
                randomNumberAsString += random.Next(1, 9).ToString();
            }
            return randomNumberAsString;
        }

        private int GenerateRandomNumber(int digitsCount) => int.Parse(GenerateRandomString(digitsCount));

        public async Task<bool> EnsureFundsAsync(CardCredentialsDTO card, double amount)
        {
            var storedData = await _cardsRepository.GetByCardNumberAsync(card.CardNumber);
            return storedData.Balance >= amount;
        }

        public async Task SetBalanceAsync(string cardNumber, double balance)
        {
            if (!await _cardsRepository.ExistsAsync(cardNumber)) throw new CardNotFoundException(cardNumber);
            await _cardsRepository.SetBalanceAsync(cardNumber, balance);
        }
    }
}
