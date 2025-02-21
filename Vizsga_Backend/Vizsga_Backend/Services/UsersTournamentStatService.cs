using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VizsgaBackend.Models;

namespace VizsgaBackend.Services
{
    public class UsersTournamentStatService
    {
        private readonly IMongoCollection<UsersTournamentStat> _usersTournamentStatCollection;

        public UsersTournamentStatService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersTournamentStatCollection = database.GetCollection<UsersTournamentStat>(mongoDBSettings.Value.UsersTournamentStatsCollectionName);
        }
        public async Task<UsersTournamentStat> GetByUserIdAsync(string userId)
        {
            return await _usersTournamentStatCollection.Find(stat => stat.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(UsersTournamentStat stat)
        {
            await _usersTournamentStatCollection.InsertOneAsync(stat);
        }

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<UsersTournamentStat> filter, UpdateDefinition<UsersTournamentStat> update)
        {
            return await _usersTournamentStatCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string userId)
        {
            await _usersTournamentStatCollection.DeleteOneAsync(stat => stat.UserId == userId);
        }

        public async Task<UsersTournamentStat> GetUpdatedValuesAsync(FilterDefinition<UsersTournamentStat> filter, UpdateDefinition<UsersTournamentStat> updateDefinition)
        {
            return await _usersTournamentStatCollection.FindOneAndUpdateAsync(filter, updateDefinition,
                new FindOneAndUpdateOptions<UsersTournamentStat>
                {
                    ReturnDocument = ReturnDocument.After
                }
            );
        }
    }
}
