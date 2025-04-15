using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Vizsga_Backend.Models.SignalRModels;
using Vizsga_Backend.Models.UserStatsModels;
using VizsgaBackend.Models;

namespace VizsgaBackend.Services
{
    public class UsersFriendlyStatService : IUsersFriendlyStatService
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

        public async Task<UpdateResult> SavePlayerStat(string userId, EndMatchModel stat)
        {
            var filter = Builders<UsersFriendlyStat>.Filter.Eq(s => s.UserId, userId);
            var existingStat = await _usersFriendlyStatCollection.Find(filter).FirstOrDefaultAsync();

            if (existingStat == null)
            {
                var newStat = new UsersFriendlyStat
                {
                    UserId = userId,
                    Matches = 1,
                    MatchesWon = stat.Won ? 1 : 0,
                    Sets = stat.Sets ?? 0,
                    SetsWon = stat.SetsWon ?? 0,
                    Legs = stat.Legs ?? 0,
                    LegsWon = stat.LegsWon ?? 0,
                    Averages = stat.Averages ?? 0,
                    Max180s = stat.Max180s ?? 0,
                    CheckoutPercentage = stat.CheckoutPercentage ?? 0,
                    HighestCheckout = stat.HighestCheckout ?? 0,
                    NineDarter = stat.NineDarter ?? 0
                };

                await _usersFriendlyStatCollection.InsertOneAsync(newStat);
                return UpdateResult.Unacknowledged.Instance;
            }

            // Átlagolt mezők frissítése: új érték beépítése az eddigiekbe
            int matchCount = existingStat.Matches ?? 0;
            double newAverage = stat.Averages ?? 0;
            double updatedAverage = matchCount > 0
                ? (((existingStat.Averages ?? 0) * matchCount) + newAverage) / (matchCount + 1)
                : newAverage;

            double newCheckout = stat.CheckoutPercentage ?? 0;
            double updatedCheckout = matchCount > 0
                ? (((existingStat.CheckoutPercentage ?? 0) * matchCount) + newCheckout) / (matchCount + 1)
                : newCheckout;

            var update = Builders<UsersFriendlyStat>.Update
                .Inc(s => s.Matches, 1)
                .Inc(s => s.MatchesWon, stat.Won ? 1 : 0)
                .Inc(s => s.Sets, stat.Sets ?? 0)
                .Inc(s => s.SetsWon, stat.SetsWon ?? 0)
                .Inc(s => s.Legs, stat.Legs ?? 0)
                .Inc(s => s.LegsWon, stat.LegsWon ?? 0)
                .Inc(s => s.Max180s, stat.Max180s ?? 0)
                .Inc(s => s.NineDarter, stat.NineDarter ?? 0)
                .Set(s => s.Averages, updatedAverage)
                .Set(s => s.CheckoutPercentage, updatedCheckout)
                .Set(s => s.HighestCheckout, Math.Max(existingStat.HighestCheckout ?? 0, stat.HighestCheckout ?? 0));

            return await _usersFriendlyStatCollection.UpdateOneAsync(filter, update);
        }

    }
}
