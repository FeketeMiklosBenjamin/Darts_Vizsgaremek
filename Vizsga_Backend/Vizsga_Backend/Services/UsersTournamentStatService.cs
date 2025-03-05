using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Vizsga_Backend.Models.UserModels;
using VizsgaBackend.Models;
using Vizsga_Backend.Models.UserStatsModels;

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

        public async Task<List<UsersTournamentStatWithUser>> GetTournamentsWithUsersAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "users" },
                    { "localField", "user_id" },
                    { "foreignField", "_id" },
                    { "as", "user" }
                }),
                new BsonDocument("$unwind", "$user")
            };

            var result = await _usersTournamentStatCollection.Aggregate<UsersTournamentStatWithUser>(pipeline).ToListAsync();
            return result;
        }

        public async Task<UsersTournamentStatWithUser?> GetTournamentWithUserByUserIdAsync(string userId)
        {
            if (!ObjectId.TryParse(userId, out var objectId))
            {
                return null;
            }

            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("user_id", objectId)),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "users" },
                    { "localField", "user_id" },
                    { "foreignField", "_id" },
                    { "as", "user" }
                }),
                new BsonDocument("$unwind", "$user")
            };

            return await _usersTournamentStatCollection.Aggregate<UsersTournamentStatWithUser>(pipeline).FirstOrDefaultAsync();
        }

        public async Task<UsersTournamentStat> GetTournamentByUserIdAsync(string userId)
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
    }
}
