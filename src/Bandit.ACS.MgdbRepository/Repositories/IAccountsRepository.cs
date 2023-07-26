using Bandit.ACS.MgdbRepository.Models;

namespace Bandit.ACS.MgdbRepository.Repositories
{
    public interface IAccountsRepository
    {
        Task<Account> GetByMailAsync(string mail);
        Task<Account> CreateAsync(Account account);
        Task<Account> GetByIdAsync(Guid guid);
        Task<Account> GetByFullName(string firstName, string lastName);
        Task<bool> ExistsAsync(Guid ownerId);
    }
}
