namespace Bandit.ACS.Commands
{
    public class PaymentRequestCommand : ICommand
    {
        public string Type { get; set; } = nameof(PaymentRequestCommand);
        public string BankId { get; set; }
        public string CardNumber { get; set; }
        public string PaymentToken { get; set; }
    }
}
