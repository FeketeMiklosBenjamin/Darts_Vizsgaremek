using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Vizsga_Backend.Models.UserModels;
using VizsgaBackend.Models;
using Vizsga_Backend.Models.UserStatsModels;
using Vizsga_Backend.Models.SignalRModels;
using Vizsga_Backend.Services;
using System.Reflection.Emit;
using Vizsga_Backend.Models.MatchModels;

namespace VizsgaBackend.Services
{
    public class UsersTournamentStatService : IUsersTournamentStatService
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

        public async Task<List<UsersTournamentStatWithUser>> GetTournamentsWithUsersNotStrictBannedAsync()
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
                new BsonDocument("$unwind", "$user"),
                new BsonDocument("$match", new BsonDocument("user.strict_ban", false))
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

        public async Task<UpdateResult> SavePlayerTournamentStatAsync(string playerId, Match matchData, EndMatchModel stat, bool tournamentWon)
        {
            var myStat = await GetTournamentByUserIdAsync(playerId);
            var opponentId = matchData.PlayerOneId == playerId ? matchData.PlayerTwoId : matchData.PlayerOneId;
            var opponentStat = await GetTournamentByUserIdAsync(opponentId);
            var filter = Builders<UsersTournamentStat>.Filter.Eq(s => s.UserId, playerId);
            var playerStat = await _usersTournamentStatCollection.Find(filter).FirstOrDefaultAsync();

            int newDartsPoints = playerStat.DartsPoints ?? 0;

            switch (playerStat.Level)
            {
                case "Advanced":
                    newDartsPoints += (stat.Won ? 200 : -150);
                    break;
                case "Professional":
                    newDartsPoints += (stat.Won ? 200 : -250);
                    break;
                case "Champion":
                    newDartsPoints += (stat.Won ? 150 : -250);
                    break;
                default:
                    newDartsPoints += (stat.Won ? 250 : -100);
                    break;
            }

            if (newDartsPoints < 0)
            {
                newDartsPoints = 0;
            }

            string newLevel = playerStat.Level;

            if (newDartsPoints >= 0 && newDartsPoints < 1500)
            {
                newLevel = "Amateur";
            }
            else if (newDartsPoints >= 1500 && newDartsPoints < 4500)
            {
                newLevel = "Advanced";
            }
            else if(newDartsPoints >= 4500 && newDartsPoints < 9000)
            {
                newLevel = "Professional";
            }
            else
            {
                newLevel = "Champion";
            }


            int matchCount = playerStat.Matches ?? 0;
            double newAverage = stat.Averages ?? 0;
            double updatedAverage = matchCount > 0
                ? (((playerStat.Averages ?? 0) * matchCount) + newAverage) / (matchCount + 1)
                : newAverage;

            double newCheckout = stat.CheckoutPercentage ?? 0;
            double updatedCheckout = matchCount > 0
                ? (((playerStat.CheckoutPercentage ?? 0) * matchCount) + newCheckout) / (matchCount + 1)
                : newCheckout;

            var update = Builders<UsersTournamentStat>.Update
                .Inc(s => s.Matches, 1)
                .Inc(s => s.MatchesWon, stat.Won ? 1 : 0)
                .Inc(s => s.Sets, stat.Sets ?? 0)
                .Inc(s => s.SetsWon, stat.SetsWon ?? 0)
                .Inc(s => s.Legs, stat.Legs ?? 0)
                .Inc(s => s.LegsWon, stat.LegsWon ?? 0)
                .Inc(s => s.Max180s, stat.Max180s ?? 0)
                .Inc(s => s.NineDarter, stat.NineDarter ?? 0)
                .Inc(s => s.TournamentsWon, tournamentWon ? 1 : 0)
                .Set(s => s.Averages, updatedAverage)
                .Set(s => s.CheckoutPercentage, updatedCheckout)
                .Set(s => s.HighestCheckout, Math.Max(playerStat.HighestCheckout ?? 0, stat.HighestCheckout ?? 0))
                .Set(s => s.DartsPoints, newDartsPoints)
                .Set(s => s.Level, newLevel);

            return await _usersTournamentStatCollection.UpdateOneAsync(filter, update);
        }

    }
}
