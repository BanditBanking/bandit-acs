using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;

namespace Bandit.ACS.Daemon.Services
{
    public interface IAccountsService
    {
        Task<bool> EnsureFundsAsync(Guid id, double amount);
        Task<ProfileDTO> GetProfile(Guid id);
        Task<SessionTokenDTO> Login(LoginDTO loginDTO, string ipAddress);
        Task<SessionTokenDTO> Register(RegisterDTO registerDTO, string ipAddress);
        Task<Account> GetByFullName(string firstName, string LastName);
    }
}
