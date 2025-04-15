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

namespace VizsgaBackend.Services
{
    public class UsersTournamentStatService : IUsersTournamentStatService
    {
        private readonly IMongoCollection<UsersTournamentStat> _usersTournamentStatCollection;
        private readonly IMatchService _matchService;

        public UsersTournamentStatService(IOptions<MongoDBSettings> mongoDBSettings, IMatchService matchService)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersTournamentStatCollection = database.GetCollection<UsersTournamentStat>(mongoDBSettings.Value.UsersTournamentStatsCollectionName);
            _matchService = matchService;
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

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<UsersTournamentStat> filter, UpdateDefinition<UsersTournamentStat> update)
        {
            return await _usersTournamentStatCollection.UpdateOneAsync(filter, update);
        }
        //public async Task<UpdateResult> SavePlayerTournamentStat(string userId, string matchId, EndMatchModel stat)
        //{
        //    var matchData = await _matchService.GetMatchByIdAsync(matchId);
        //    var myStat = await GetTournamentByUserIdAsync(userId);
        //    var opponentId = matchData.PlayerOneId == userId ? matchData.PlayerTwoId : matchData.PlayerOneId;
        //    var opponentStat = await GetTournamentByUserIdAsync(opponentId);
        //    var filter = Builders<UsersTournamentStat>.Filter.Eq(s => s.UserId, userId);
        //    var existingStat = await _usersTournamentStatCollection.Find(filter).FirstOrDefaultAsync();

        //    if (existingStat == null)
        //    {
        //        var newStat = new UsersTournamentStat
        //        {
        //            UserId = userId,
        //            Matches = 1,
        //            MatchesWon = stat.Won ? 1 : 0,
        //            Sets = stat.Sets ?? 0,
        //            SetsWon = stat.SetsWon ?? 0,
        //            Legs = stat.Legs ?? 0,
        //            LegsWon = stat.LegsWon ?? 0,
        //            Averages = stat.Averages ?? 0,
        //            Max180s = stat.Max180s ?? 0,
        //            CheckoutPercentage = stat.CheckoutPercentage ?? 0,
        //            HighestCheckout = stat.HighestCheckout ?? 0,
        //            NineDarter = stat.NineDarter ?? 0,
        //            TournamentsWon = tournamentWon ? 1 : 0,
        //            DartsPoints = dartsPoints,
        //            Level = level
        //        };

        //        await _usersTournamentStatCollection.InsertOneAsync(newStat);
        //        return UpdateResult.Unacknowledged.Instance;
        //    }

        //    int matchCount = existingStat.Matches ?? 0;
        //    double newAverage = stat.Averages ?? 0;
        //    double updatedAverage = matchCount > 0
        //        ? (((existingStat.Averages ?? 0) * matchCount) + newAverage) / (matchCount + 1)
        //        : newAverage;

        //    double newCheckout = stat.CheckoutPercentage ?? 0;
        //    double updatedCheckout = matchCount > 0
        //        ? (((existingStat.CheckoutPercentage ?? 0) * matchCount) + newCheckout) / (matchCount + 1)
        //        : newCheckout;

        //    var update = Builders<UsersFriendlyStat>.Update
        //        .Inc(s => s.Matches, 1)
        //        .Inc(s => s.MatchesWon, stat.Won ? 1 : 0)
        //        .Inc(s => s.Sets, stat.Sets ?? 0)
        //        .Inc(s => s.SetsWon, stat.SetsWon ?? 0)
        //        .Inc(s => s.Legs, stat.Legs ?? 0)
        //        .Inc(s => s.LegsWon, stat.LegsWon ?? 0)
        //        .Inc(s => s.Max180s, stat.Max180s ?? 0)
        //        .Inc(s => s.NineDarter, stat.NineDarter ?? 0)
        //        .Set(s => s.Averages, updatedAverage)
        //        .Set(s => s.CheckoutPercentage, updatedCheckout)
        //        .Set(s => s.HighestCheckout, Math.Max(existingStat.HighestCheckout ?? 0, stat.HighestCheckout ?? 0));

        //    return await _usersFriendlyStatCollection.UpdateOneAsync(filter, update);
        //}

    }
}
