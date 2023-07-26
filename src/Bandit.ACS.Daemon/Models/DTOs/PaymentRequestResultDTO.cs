namespace Bandit.ACS.Daemon.Models.DTOs
{
    public class PaymentRequestResultDTO
    {
        public bool IsSuccess { get; set; }
        public AnalyticsTransaction Transaction { get; internal set; }
    }
}
