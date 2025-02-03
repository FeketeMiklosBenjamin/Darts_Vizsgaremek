using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using VizsgaBackend.Models;

namespace VizsgaBackend.Services
{
    public class UserFriendlyStatService
    {
        private readonly IMongoCollection<UserFriendlyStat> _usersFriendlyStatCollection;

        public UserFriendlyStatService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersFriendlyStatCollection = database.GetCollection<UserFriendlyStat>(mongoDBSettings.Value.UsersFriendlyStatsCollectionName);
        }
        public async Task<UserFriendlyStat> GetByUserIdAsync(string userId)
        {
            return await _usersFriendlyStatCollection.Find(stat => stat.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(UserFriendlyStat stat)
        {
            await _usersFriendlyStatCollection.InsertOneAsync(stat);
        }

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<UserFriendlyStat> filter, UpdateDefinition<UserFriendlyStat> update)
        {
            return await _usersFriendlyStatCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string userId)
        {
            await _usersFriendlyStatCollection.DeleteOneAsync(stat => stat.UserId == userId);
        }
    }
}
