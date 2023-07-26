namespace Bandit.ACS.Daemon.Models.DTOs
{
    public class EidChallengeAttemptDTO
    {
        public string Cert { get; set; } = string.Empty;
        public Guid Token { get; set; } = Guid.Empty;
        public string TokenSignature { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty; // SHA256withRSA - SHA384withECDSA
    }
}
