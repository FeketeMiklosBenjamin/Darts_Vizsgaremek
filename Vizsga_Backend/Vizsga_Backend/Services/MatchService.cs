using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.TournamentModels;
using VizsgaBackend.Models;

namespace Vizsga_Backend.Services
{
    public class MatchService
    {
        private readonly IMongoCollection<Match> _matchCollection;

        public MatchService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _matchCollection = database.GetCollection<Match>(mongoDBSettings.Value.MatchesCollectionName);
        }

        public async Task CreateMatchAsync(Match match)
        {
            await _matchCollection.InsertOneAsync(match);
        }

        public async Task<MatchWithPlayers?> GetMatchByIdAsync(string matchId)
        {
            var pipeline = new[]
            {
                new BsonDocument
                {
                    { "$match", new BsonDocument { { "_id", new ObjectId(matchId) } } }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "users" },
                            { "localField", "player_one_id" },
                            { "foreignField", "_id" },
                            { "as", "player_one" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "users" },
                            { "localField", "player_two_id" },
                            { "foreignField", "_id" },
                            { "as", "player_two" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "player_match_stats" },
                            { "localField", "player_one_stat_id" },
                            { "foreignField", "_id" },
                            { "as", "player_one_stat" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "player_match_stats" },
                            { "localField", "player_two_stat_id" },
                            { "foreignField", "_id" },
                            { "as", "player_two_stat" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$addFields", new BsonDocument
                        {
                            { "player_one", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_one", 0 } } } },
                            { "player_two", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_two", 0 } } } },
                            { "player_one_stat", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_one_stat", 0 } } } },
                            { "player_two_stat", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_two_stat", 0 } } } }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$unset", new BsonArray { "player_one_id", "player_two_id", "player_one_stat_id", "player_two_stat_id" } }
                }
            };

            return await _matchCollection.Aggregate<MatchWithPlayers>(pipeline).FirstOrDefaultAsync();
        }

        public async Task<MatchWithPlayers?> GetUserUpcomingMatchAsync(string userId)
        {
            var userObjectId = new ObjectId(userId);

            var pipeline = new[]
            {
                new BsonDocument
                {
                    { "$match", new BsonDocument
                        {
                            { "$or", new BsonArray
                                {
                                    new BsonDocument { { "player_one_id", userObjectId } },
                                    new BsonDocument { { "player_two_id", userObjectId } }
                                }
                            },
                            { "start_date", new BsonDocument { { "$gt", DateTime.UtcNow } } },
                            { "status", new BsonDocument { { "$ne", "Finished" } } }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$sort", new BsonDocument { { "start_date", 1 } } }
                },
                new BsonDocument
                {
                    { "$limit", 1 }
                },
                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "users" },
                            { "localField", "player_one_id" },
                            { "foreignField", "_id" },
                            { "as", "player_one_info" }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "users" },
                            { "localField", "player_two_id" },
                            { "foreignField", "_id" },
                            { "as", "player_two_info" }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$addFields", new BsonDocument
                        {
                            { "player_one", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_one_info", 0 } } } },
                            { "player_two", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_two_info", 0 } } } }
                        }
                    }
                }
            };
            return await _matchCollection.Aggregate<MatchWithPlayers>(pipeline).FirstOrDefaultAsync();
        }

        public async Task<List<MatchWithPlayers>> GetUserLastMatchesAsync(string userId, int matchesCount)
        {
            var userObjectId = new ObjectId(userId);

            var pipeline = new[]
            {
                new BsonDocument
                {
                    { "$match", new BsonDocument
                        {
                            { "$or", new BsonArray
                                {
                                    new BsonDocument { { "player_one_id", userObjectId } },
                                    new BsonDocument { { "player_two_id", userObjectId } }
                                }
                            },
                            { "status", "Finished" }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$sort", new BsonDocument { { "start_date", -1 } } }
                },
                new BsonDocument
                {
                    { "$limit", matchesCount }
                },
                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "users" },
                            { "localField", "player_one_id" },
                            { "foreignField", "_id" },
                            { "as", "player_one_info" }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "users" },
                            { "localField", "player_two_id" },
                            { "foreignField", "_id" },
                            { "as", "player_two_info" }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$addFields", new BsonDocument
                        {
                            { "player_one", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_one_info", 0 } } } },
                            { "player_two", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_two_info", 0 } } } }
                        }
                    }
                }
            };

            return await _matchCollection.Aggregate<MatchWithPlayers>(pipeline).ToListAsync();
        }
    }
}
