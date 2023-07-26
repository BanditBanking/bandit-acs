using Bandit.ACS.Daemon.Models;
using Transaction = Bandit.ACS.Daemon.Models.Transaction;

namespace Bandit.ACS.Daemon.Mappers
{
    public static class TransactionExtensions
    {
        public static Transaction ToContract(this NpgsqlRepository.Models.Transaction transaction) => new()
        {
            TransactionId = transaction.Id,
            MerchantId = transaction.MerchantId,
            MerchantName = transaction.MerchantName,
            Amount = transaction.Amount,
            RequestTime = transaction.RequestTime,
            Status = (Models.TransactionStatus) transaction.Status,
            ChallengeType = (Models.ChallengeType) transaction.ChallengeType
        };

        public static AnalyticsTransaction ToAnalytics(this NpgsqlRepository.Models.Transaction transaction, MgdbRepository.Models.Account account) => new()
        {
            Id = transaction.Id,
            DebitBank = transaction.DebitBank,
            CreditBank = transaction.MerchantBank,
            ClientId = transaction.AccountId,
            ClientGender = account.Gender,
            ClientBirthDate = account.BirthDay,
            ClientAge = (int)(DateTime.Now.Subtract(account.BirthDay).TotalDays / 365.25),
            ClientMaritalStatus = account.MaritalStatus,
            ClientMonthlySalary = account.MonthlySalary,
            TransactionDate = transaction.RequestTime,
            MerchantId = transaction.MerchantId,
            MerchantActivity = transaction.ActivitySector,
            AuthenticationMethod = transaction.ChallengeType.ToString(),
            TransferredAmount = transaction.Amount
        };
    }
}
