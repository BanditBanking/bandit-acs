namespace Bandit.ACS.NpgsqlRepository.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string ActivitySector { get; set; }
        public ChallengeType ChallengeType { get; set; }
        public double Amount { get; set; }
        public string? Communication { get; set; }
        public string RequestIp { get; set; }
        public DateTime RequestTime { get; set; }
        public TransactionStatus Status { get; set; }
        public string PaymentToken { get; set; }
        public string? MerchantBank { get; set; }
        public string? MerchantCardNumber { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string DebitBank { get; set; }
    }
}
