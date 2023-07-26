using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bandit.ACS.MgdbRepository.Models
{
    public class Account
    {
        [Key]
        [Required]
        [BsonId]
        public ObjectId Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Mail { get; set; }

        [Required]
        [MaxLength(72)]
        public string Password { get; set; }

        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;

        [Required]
        public bool IsAdmin { get; set; } = false;

        public DateTime BirthDay { get; set; }

        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public double MonthlySalary { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
