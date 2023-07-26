using MongoDB.Driver;

namespace Bandit.ACS.MgdbRepository.Utils
{
    public static class MgdbDatabaseFactory
    {
        public static IMongoDatabase Create(string connectionString, string databaseName)
        {
            var mongoClient = new MongoClient(connectionString);
            return mongoClient.GetDatabase(databaseName);
        }
    }
}
