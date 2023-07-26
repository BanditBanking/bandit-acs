using Bandit.ACS.Daemon.Models.DTOs;
using Account = Bandit.ACS.Daemon.Models.Account;

namespace Bandit.ACS.Daemon.Mappers
{
    public static class AccountExtensions
    {
        public static MgdbRepository.Models.Account ToModel(this Account account) => new()
        {
            UserId = Guid.NewGuid(),
            Mail = account.Mail,
            Password = account.Password,
            FirstName = account.FirstName,
            LastName = account.LastName,
            BirthDay = account.BirthDay,
            Gender = account.Gender,
            MaritalStatus = account.MaritalStatus,
            MonthlySalary = account.MonthlySalary,
            PhoneNumber = account.PhoneNumber,
        };

        public static MgdbRepository.Models.Account ToModel(this RegisterDTO registerDto) => new()
        {
            UserId = Guid.NewGuid(),
            Mail = registerDto.Mail,
            Password = registerDto.Password,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            BirthDay = registerDto.BirthDay,
            Gender = registerDto.Gender,
            MaritalStatus = registerDto.MaritalStatus,
            MonthlySalary = registerDto.MonthlySalary,
            PhoneNumber = registerDto.PhoneNumber,
        };

        public static Account ToContract(this MgdbRepository.Models.Account account) => new()
        {
            Id = account.UserId,
            Mail = account.Mail,
            FirstName = account.FirstName,
            LastName = account.LastName,
            IsAdmin = account.IsAdmin,
            BirthDay = account.BirthDay,
            Gender = account.Gender,
            PhoneNumber = account.PhoneNumber,
        };
    }
}
