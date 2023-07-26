namespace Bandit.ACS.Daemon.Models
{
    public class Card
    {
        public Guid OwnerId { get; set; }
        public string CardNumber { get; set; }
        public int PinCode { get; set; }
        public double Balance { get; set; }
    }
}
