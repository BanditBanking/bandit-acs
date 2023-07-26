using Bandit.ACS.MgdbRepository.Exceptions;
using Bandit.ACS.MgdbRepository.Repositories;
using Bandit.ACS.Daemon.Mappers;
using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Exceptions;
using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Services
{
    public class AccountsService : IAccountsService
    {
        private IAccountsRepository _accountsRepository;
        private ICardsRepository _cardRepository;
        private ITokenService _tokenService;
        private ICardsService _cardService;

        public AccountsService(IAccountsRepository accountsRepository, ICardsRepository cardRepository, ITokenService tokenService, ICardsService cardService)
        {
            _accountsRepository = accountsRepository;
            _cardRepository = cardRepository;
            _tokenService = tokenService;
            _cardService = cardService;
        }

        public async Task<SessionTokenDTO> Login(LoginDTO loginDTO, string ipAddress)
        {
            try
            {
                var account = await _accountsRepository.GetByMailAsync(loginDTO.Mail);
                if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, account.Password))
                    throw new InvalidCredentialsException();
                return _tokenService.GenerateToken(account.UserId, account.Mail, account.IsAdmin ? Role.SingleFactorAdmin : Role.SingleFactorUser);
            } 
            catch(ResourceNotFoundException)
            {
                throw new InvalidCredentialsException();
            }
        }

        public async Task<SessionTokenDTO> Register(RegisterDTO registerDTO, string ipAddress)
        {
            try
            {
                registerDTO.Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
                var registeredAccount = await _accountsRepository.CreateAsync(registerDTO.ToModel());
                var card = await _cardService.GenerateCardAsync(registeredAccount.UserId);
                return _tokenService.GenerateToken(registeredAccount.UserId, registeredAccount.Mail, registeredAccount.IsAdmin ? Role.SingleFactorAdmin : Role.SingleFactorUser);
            } 
            catch(AccountAlreadyRegisteredException)
            {
                throw new Exceptions.AccountAlreadyRegisteredException(registerDTO.Mail);
            }
        }

        public async Task<ProfileDTO> GetProfile(Guid id)
        {
            var account = await _accountsRepository.GetByIdAsync(id);
            var card = await _cardRepository.GetCardByOwnerAsync(id);
            return new ProfileDTO { Account = account.ToContract(), Card = card.ToContract() };
        }

        public async Task<bool> EnsureFundsAsync(Guid id, double amount)
        {
            var card = await _cardRepository.GetCardByOwnerAsync(id);
            return card.Balance >= amount;
        }

        public async Task<Account> GetByFullName(string firstName, string lastName)
        {
            var account = await _accountsRepository.GetByFullName(firstName, lastName);
            return account.ToContract();
        }
    }
}
