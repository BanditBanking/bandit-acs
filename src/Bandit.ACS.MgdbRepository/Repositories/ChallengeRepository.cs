using Bandit.ACS.MgdbRepository.Exceptions;
using Bandit.ACS.MgdbRepository.Models;
using MongoDB.Driver;

namespace Bandit.ACS.MgdbRepository.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly IMongoCollection<Challenge> _challenges;

        public ChallengeRepository(IMongoDatabase database)
        {
            _challenges = database.GetCollection<Challenge>("challenges");
        }

        public async Task AddChallengeAsync(Challenge challenge) => await _challenges.InsertOneAsync(challenge);

        public async Task<List<Challenge>> GetAfterDateAsync(DateTime date) => await _challenges.Find(c => c.RequestTime > date).ToListAsync();

        public async Task<Challenge> GetByIdAsync(Guid challengeId)
        {
            var challenges = await _challenges.Find(_ => true).ToListAsync();
            var challenge = await _challenges.Find(c => c.ChallengeId == challengeId).FirstOrDefaultAsync();

            if (challenge == null)
                throw new ResourceNotFoundException($"A challenge with id {challengeId} could not be found");

            return challenge;
        }

        public async Task UpdateChallengeAsync(Challenge challenge) => await _challenges.ReplaceOneAsync((a) => a.ChallengeId == challenge.ChallengeId, challenge);
    }
}
