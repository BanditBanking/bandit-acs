namespace Bandit.ACS.NpgsqlRepository.Models
{
    public enum TransactionStatus
    {
        Requested,
        Authorized,
        Aborted,
        Completed
    }
}
