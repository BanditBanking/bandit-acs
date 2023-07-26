using System.ComponentModel.DataAnnotations;

namespace Bandit.ACS.Daemon.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
