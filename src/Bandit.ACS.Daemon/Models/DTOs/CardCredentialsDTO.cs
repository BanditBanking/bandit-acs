namespace Bandit.ACS.Daemon.Models.DTOs
{
    public class CardCredentialsDTO
    {
        public string CardNumber { get; set; }
        public int CVV { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
    }
}
