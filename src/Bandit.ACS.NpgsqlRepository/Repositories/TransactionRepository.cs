using Bandit.ACS.NpgsqlRepository.Exceptions;
using Bandit.ACS.NpgsqlRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace Bandit.ACS.NpgsqlRepository.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly NpgsqlDbContext _context;
        private readonly DbSet<Transaction> _transactions;

        public TransactionRepository(NpgsqlDbContext context)
        {
            _context = context;
            _transactions = context.Transactions;
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetByAccountId(Guid id) => await _transactions.Where(t => t.AccountId == id).ToListAsync();

        public async Task<Transaction> GetByIdAsync(Guid transactionId)
        {
            var transaction = await _transactions.Where(t => t.Id == transactionId).FirstOrDefaultAsync();

            if (transaction == null)
                throw new ResourceNotFoundException($"No transaction with id {transactionId}");

            return transaction;
        }

        public async Task<Transaction?> GetByPaymentToken(string paymentToken) => await _transactions.Where(t => t.PaymentToken == paymentToken).FirstOrDefaultAsync();

        public async Task UpdateAsync(Transaction transaction)
        {
            _transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
