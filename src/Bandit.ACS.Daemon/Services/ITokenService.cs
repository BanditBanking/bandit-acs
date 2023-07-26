using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Models.DTOs;
using MongoDB.Bson;

namespace Bandit.ACS.Daemon.Services
{
    public interface ITokenService
    {
        SessionTokenDTO GenerateToken(Guid userId, string mail, Role role);
        Task<Account> GetAccountAsync(string? token);
    }
}
