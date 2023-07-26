using System.ComponentModel.DataAnnotations;

namespace Bandit.ACS.Daemon.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Mail { get; set; }

        [Required]
        [MaxLength(72)]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public int Age { get => (int)(DateTime.Now.Subtract(BirthDay).TotalDays / 365.25); }
        public string Gender { get; set; }
        public bool IsAdmin { get; set; }
        public string MaritalStatus { get; internal set; }
        public double MonthlySalary { get; internal set; }
        public string PhoneNumber { get; internal set; } = string.Empty;
    }
}
