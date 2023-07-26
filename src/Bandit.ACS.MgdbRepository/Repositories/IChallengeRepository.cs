using Bandit.ACS.MgdbRepository.Models;

namespace Bandit.ACS.MgdbRepository.Repositories
{
    public interface IChallengeRepository
    {
        Task AddChallengeAsync(Challenge challenge);
        Task<List<Challenge>> GetAfterDateAsync(DateTime dateTime);
        Task<Challenge> GetByIdAsync(Guid challengeId);
        Task UpdateChallengeAsync(Challenge challenge);
    }
}
