using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Bandit.ACS.MgdbRepository.Models
{
    public class Card
    {
        [Key]
        [Required]
        [BsonId]
        public ObjectId Id { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public int PinCode { get; set; }

        [Required]
        public double Balance { get; set; }
    }
}
