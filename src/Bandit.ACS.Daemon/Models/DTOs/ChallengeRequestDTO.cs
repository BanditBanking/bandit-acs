namespace Bandit.ACS.Daemon.Models.DTOs
{
    public class ChallengeRequestDTO
    {
        public Guid MerchantId { get; set; }
        public double Amount { get; set; }
        public Uri RedirectUrl { get; set; }
        public string? Communication { get; set; }
        public CardCredentialsDTO CardCredentials { get; set; }
    }
}
