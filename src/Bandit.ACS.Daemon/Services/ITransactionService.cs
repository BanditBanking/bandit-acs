using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;
using Account = Bandit.ACS.Daemon.Models.Account;

namespace Bandit.ACS.Daemon.Services
{
    public interface ITransactionService
    {
        Task<string> RequestTransactionAsync(Account challenger, TransactionRequestDTO request, string ip);
        Task AuthorizeTransactionAsync(Guid authorizedTransactionId);
        Task<List<Transaction>> GetTransactionsByAccountId(Guid id);
        Task<AnalyticsTransaction> RequestPaymentAsync(string bankId, string cardNumber, string paymentToken);
    }
}
