using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VizsgaBackend.Models;

namespace VizsgaBackend.Services
{
    public class UserTournamentStatService
    {
        private readonly IMongoCollection<UserTournamentStat> _usersTournamentStatCollection;

        public UserTournamentStatService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersTournamentStatCollection = database.GetCollection<UserTournamentStat>(mongoDBSettings.Value.UsersTournamentStatsCollectionName);
        }
        public async Task<UserTournamentStat> GetByUserIdAsync(string userId)
        {
            return await _usersTournamentStatCollection.Find(stat => stat.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(UserTournamentStat stat)
        {
            await _usersTournamentStatCollection.InsertOneAsync(stat);
        }

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<UserTournamentStat> filter, UpdateDefinition<UserTournamentStat> update)
        {
            return await _usersTournamentStatCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string userId)
        {
            await _usersTournamentStatCollection.DeleteOneAsync(stat => stat.UserId == userId);
        }

        public async Task<UserTournamentStat> GetUpdatedValuesAsync(FilterDefinition<UserTournamentStat> filter, UpdateDefinition<UserTournamentStat> updateDefinition)
        {
            return await _usersTournamentStatCollection.FindOneAndUpdateAsync(filter, updateDefinition,
                new FindOneAndUpdateOptions<UserTournamentStat>
                {
                    ReturnDocument = ReturnDocument.After
                }
            );
        }
    }
}
