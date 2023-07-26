namespace Bandit.ACS.Daemon.Models
{
    public class AnalyticsChallenge
    {
        public Guid ChallengeId { get; set; }   
        public string ChallengeType { get; set; }
        public string BankId { get; set; } // OTP
        public Guid ClientId { get; set; } // OTP
        public DateTime BirtDate { get; set; } // OTP
        public int Age { get; set; } // OTP
        public string Gender { get; set; } // OTP
        public DateTime RequestTime { get; set; } // OTP
        public int AttemptCount { get; set; } // OTP
        public DateTime ResponseTime { get; set; } // OTP
        public string Decision { get; set; } // OTP
        public bool MaxAttemptsReached { get; set; } // OTP
        public DateTime DecisionTime { get; set; } // OTP
    }
}
