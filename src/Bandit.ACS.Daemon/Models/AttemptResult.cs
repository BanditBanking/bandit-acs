using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Models
{
    public class AttemptResult
    {
        public Guid Id { get; set; }
        public bool IsSuccess { get; set; }
        public int RemainingAttempts { get; set; }
        public SessionTokenDTO? SessionToken { get; set; }
    }
}
