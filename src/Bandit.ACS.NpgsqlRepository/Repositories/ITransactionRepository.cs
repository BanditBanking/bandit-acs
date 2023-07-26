using Bandit.ACS.NpgsqlRepository.Models;

namespace Bandit.ACS.NpgsqlRepository.Repositories
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetByAccountId(Guid id);
        Task<Transaction> GetByIdAsync(Guid transactionId);
        Task<Transaction?> GetByPaymentToken(string paymentToken);
        Task UpdateAsync(Transaction transaction);
    }
}
