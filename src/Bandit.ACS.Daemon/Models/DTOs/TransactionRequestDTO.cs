namespace Bandit.ACS.Daemon.Models.DTOs
{
    public class TransactionRequestDTO
    {
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string ActivitySector { get; set; }
        public double Amount { get; set; }
        public ChallengeType ChallengeType { get; set; }
        public string? Communication { get; set; }
    }
}
