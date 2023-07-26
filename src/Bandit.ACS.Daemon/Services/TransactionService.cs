using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.NpgsqlRepository.Repositories;
using Bandit.ACS.Exceptions;
using Account = Bandit.ACS.Daemon.Models.Account;
using Transaction = Bandit.ACS.NpgsqlRepository.Models.Transaction;
using Bandit.ACS.Daemon.Mappers;
using Bandit.ACS.Daemon.Exceptions;
using Bandit.ACS.Daemon.Models;
using ChallengeType = Bandit.ACS.NpgsqlRepository.Models.ChallengeType;
using Bandit.ACS.MgdbRepository.Repositories;
using Bandit.ACS.Configuration;

namespace Bandit.ACS.Daemon.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountsService _accountsService;
        private readonly IAccountsRepository _accountsRepository;
        private readonly DaemonConfiguration _config;

        public TransactionService(ITransactionRepository transactionRepository, IAccountsService accountsService, IAccountsRepository accountsRepository, DaemonConfiguration config)
        {
            _transactionRepository = transactionRepository;
            _accountsService = accountsService;
            _accountsRepository = accountsRepository;
            _config = config;
        }

        public async Task AuthorizeTransactionAsync(Guid transactionId)
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);
            transaction.Status = NpgsqlRepository.Models.TransactionStatus.Authorized;
            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task<string> RequestTransactionAsync(Account challenger, TransactionRequestDTO request, string ip)
        {
            if (!await _accountsService.EnsureFundsAsync(challenger.Id, request.Amount)) throw new InsufficientFundsException();

            var transactionId = Guid.NewGuid();

            var newTransaction = new Transaction
            {
                Id = transactionId,
                AccountId = challenger.Id,
                DebitBank = _config.BankIdentifier,
                Amount = request.Amount,
                MerchantId = request.MerchantId,
                MerchantName = request.MerchantName,
                ActivitySector = request.ActivitySector,
                ChallengeType = (ChallengeType) request.ChallengeType,
                Communication = request.Communication,
                RequestTime = DateTime.UtcNow,
                RequestIp = ip,
                Status = NpgsqlRepository.Models.TransactionStatus.Requested,
                PaymentToken = Guid.NewGuid().ToString(),
            };

            await _transactionRepository.AddTransactionAsync(newTransaction);

            return newTransaction.PaymentToken;
        }

        public async Task<List<Models.Transaction>> GetTransactionsByAccountId(Guid id)
        {
            var transactions = await _transactionRepository.GetByAccountId(id);
            return transactions.Select(t => t.ToContract()).ToList();
        }

        public async Task<AnalyticsTransaction> RequestPaymentAsync(string bankId, string cardNumber, string paymentToken)
        {
            var transaction = await _transactionRepository.GetByPaymentToken(paymentToken) ?? throw new InvalidPaymentTokenException(paymentToken);
            transaction.MerchantBank = bankId;
            transaction.MerchantCardNumber = cardNumber;
            transaction.CompletionDate = DateTime.UtcNow;
            transaction.Status = NpgsqlRepository.Models.TransactionStatus.Completed;
            await _transactionRepository.UpdateAsync(transaction);
            var account = await _accountsRepository.GetByIdAsync(transaction.AccountId);
            return transaction.ToAnalytics(account);
        }
    }
}
