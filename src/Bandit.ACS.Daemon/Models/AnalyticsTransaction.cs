namespace Bandit.ACS.Daemon.Models
{
    public class AnalyticsTransaction
    {
        public Guid Id { get; set; }
        public string DebitBank { get; set; }
        public string CreditBank { get; set; }
        public string MerchantName { get; set; }
        public Guid ClientId { get; set; }
        public string ClientGender { get; set; }
        public DateTime ClientBirthDate { get; set; }
        public int ClientAge { get; set; }
        public string ClientMaritalStatus { get; set; }
        public double ClientMonthlySalary { get; set; }
        public DateTime TransactionDate { get; set; }
        public string MerchantId { get; set; }
        public string MerchantActivity { get; set; }
        public string AuthenticationMethod { get; set; }
        public double TransferredAmount { get; set; }
    }
}
