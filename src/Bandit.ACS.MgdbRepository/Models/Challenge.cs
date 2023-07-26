using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Bandit.ACS.MgdbRepository.Models
{
    public class Challenge
    {
        [Key]
        [Required]
        [BsonId]
        public ObjectId Id { get; set; }
        public Guid ChallengeId { get; set; }
        public string OTP { get; set; }
        public Guid ChallengerId { get; set; }
        public string ChallengerIp { get; set; }
        public string ServerIp { get; set; }
        public DateTime RequestTime { get; set; }
        public string BankId { get; set; }
        public string ClientName { get; set; }
        public string CardNumber { get; set; }
        public DateTime ChallengerBirthDay { get; set; }
        public int ChallengerAge { get; set; }
        public string ChallengerGender { get; set; }
        public int RemainingAttempts { get; set; }
        public bool IsSuccess { get; set; }
        public ChallengeType ChallengeType { get; set; }
        public Guid LinkedTransactionId { get; set; }
        public DateTime ResponseTime { get; set; }
        public DateTime DecisionTime { get; set; }
    }
}
