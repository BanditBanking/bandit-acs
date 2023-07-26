using MongoDB.Bson;

namespace Bandit.ACS.Daemon.Models.DTOs
{
    public class SessionTokenDTO
    {
        public Guid UserId { get; set; }
        public string? Mail { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
