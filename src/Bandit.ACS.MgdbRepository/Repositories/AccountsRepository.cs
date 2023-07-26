using Bandit.ACS.MgdbRepository.Exceptions;
using Bandit.ACS.MgdbRepository.Models;
using MongoDB.Driver;

namespace Bandit.ACS.MgdbRepository.Repositories
{
    public class AccountRepository : IAccountsRepository
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountRepository(IMongoDatabase database)
        { 
            _accounts = database.GetCollection<Account>("accounts");
        }

        public async Task<Account> GetByMailAsync(string mail)
        {
            var account = await _accounts.Find(a => a.Mail == mail).FirstOrDefaultAsync();

            if (account == null)
                throw new ResourceNotFoundException($"A user with mail address {mail} could not be found");

            return account;
        }

        public async Task<Account> CreateAsync(Account account)
        {
            if(await _accounts.Find(a => a.Mail == account.Mail).AnyAsync())
                throw new AccountAlreadyRegisteredException(account.Mail);

            await _accounts.InsertOneAsync(account);
            return account;
        }

        public async Task<Account> GetByIdAsync(Guid id)
        {
            var account = await _accounts.Find(a => a.UserId == id).FirstOrDefaultAsync();

            if (account == null)
                throw new ResourceNotFoundException($"A user with id {id} could not be found");

            return account;
        }

        public async Task<bool> ExistsAsync(Guid ownerId) => await _accounts.Find(a => a.UserId == ownerId).AnyAsync();

        public async Task<Account> GetByFullName(string firstName, string lastName)
        {
            var account = await _accounts.Find(a => a.FirstName == firstName && a.LastName == lastName).FirstOrDefaultAsync();

            if (account == null)
                throw new ResourceNotFoundException($"A user with name {firstName + " " + lastName} could not be found");

            return account;
        }
    }
}
