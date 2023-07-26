namespace Bandit.ACS.Daemon.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public double Amount { get; set; }
        public DateTime RequestTime { get; set; }
        public TransactionStatus Status { get; set; }
        public ChallengeType ChallengeType { get; set; }

    }
}
