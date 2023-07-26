namespace Bandit.ACS.Daemon.Models.DTOs
{
    public class RegisterDTO
    {
        public string Mail { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public double MonthlySalary { get; set; }
        public string PhoneNumber { get; set; }
    }
}
