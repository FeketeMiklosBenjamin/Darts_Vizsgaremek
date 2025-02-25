using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Vizsga_Backend.Models.UserStatsModels;
using VizsgaBackend.Models;

namespace VizsgaBackend.Services
{
    public class UsersFriendlyStatService
    {
        private readonly IMongoCollection<UsersFriendlyStat> _usersFriendlyStatCollection;

        public UsersFriendlyStatService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersFriendlyStatCollection = database.GetCollection<UsersFriendlyStat>(mongoDBSettings.Value.UsersFriendlyStatsCollectionName);
        }
        public async Task<UsersFriendlyStat> GetByUserIdAsync(string userId)
        {
            return await _usersFriendlyStatCollection.Find(stat => stat.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(UsersFriendlyStat stat)
        {
            await _usersFriendlyStatCollection.InsertOneAsync(stat);
        }

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<UsersFriendlyStat> filter, UpdateDefinition<UsersFriendlyStat> update)
        {
            return await _usersFriendlyStatCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string userId)
        {
            await _usersFriendlyStatCollection.DeleteOneAsync(stat => stat.UserId == userId);
        }
    }
}
