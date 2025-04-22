using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.SignalRModels;
using Vizsga_Backend.Models.TournamentModels;
using Vizsga_Backend.Models.UserStatsModels;
using VizsgaBackend.Models;
using VizsgaBackend.Services;

namespace Vizsga_Backend.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMongoCollection<Match> _matchCollection;

        private readonly IMongoCollection<PlayerMatchStat> _playerMatchStatCollection;

        private readonly IUsersTournamentStatService _usersTournamentStatService;

        private readonly IMatchHeaderService _matchHeaderService;

        public MatchService(IOptions<MongoDBSettings> mongoDBSettings, IUsersTournamentStatService usersTournamentStatService, IMatchHeaderService matchHeaderService)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _matchCollection = database.GetCollection<Match>(mongoDBSettings.Value.MatchesCollectionName);
            _playerMatchStatCollection = database.GetCollection<PlayerMatchStat>(mongoDBSettings.Value.Players_Match_StatsCollectionName);
            _usersTournamentStatService = usersTournamentStatService;
            _matchHeaderService = matchHeaderService;
        }

        public async Task CreateMatchAsync(Match match)
        {
            await _matchCollection.InsertOneAsync(match);
        }

        public async Task<Match> GetMatchByIdAsync(string matchId)
        {
            return await _matchCollection.Find(m => m.Id == matchId).FirstOrDefaultAsync();
        }

        // Frontend
        public async Task SetAllPlayerStatNotAppearedAsync(string matchId, string? notApppearedId)
        {
            var match = await _matchCollection.Find(m => m.Id == matchId).FirstOrDefaultAsync();
            if (match == null) return;

            if (string.IsNullOrEmpty(notApppearedId))
            {
                var stat1 = new PlayerMatchStat
                {
                    Appeared = false,
                    Won = false,
                    SetsWon = null,
                    LegsWon = null,
                    Averages = null,
                    Max180s = null,
                    CheckoutPercentage = null,
                    HighestCheckout = null,
                    NineDarter = null
                };

                var stat2 = new PlayerMatchStat
                {
                    Appeared = false,
                    Won = false,
                    SetsWon = null,
                    LegsWon = null,
                    Averages = null,
                    Max180s = null,
                    CheckoutPercentage = null,
                    HighestCheckout = null,
                    NineDarter = null
                };

                await _playerMatchStatCollection.InsertOneAsync(stat1);
                await _playerMatchStatCollection.InsertOneAsync(stat2);

                var rand = new Random();
                bool playerOneWon = rand.Next(2) == 0;

                var winnerStatId = playerOneWon ? stat1.Id : stat2.Id;
                var loserStatId = playerOneWon ? stat2.Id : stat1.Id;

                await _playerMatchStatCollection.UpdateOneAsync(s => s.Id == winnerStatId, Builders<PlayerMatchStat>.Update.Set(s => s.Won, true));

                var update = Builders<Match>.Update
                    .Set(m => m.PlayerOneStatId, match.PlayerOneId == (playerOneWon ? match.PlayerOneId : match.PlayerTwoId) ? stat1.Id : stat2.Id)
                    .Set(m => m.PlayerTwoStatId, match.PlayerTwoId == (playerOneWon ? match.PlayerTwoId : match.PlayerOneId) ? stat2.Id : stat1.Id)
                    .Set(m => m.Status, "Finished");

                await _matchCollection.UpdateOneAsync(m => m.Id == match.Id, update);

                await CreateNextMatchAsync(match, (playerOneWon ? match.PlayerOneId! : match.PlayerTwoId!));
            }
            else
            {
                var stat = new PlayerMatchStat
                {
                    Appeared = false,
                    Won = false,
                    SetsWon = null,
                    LegsWon = null,
                    Averages = null,
                    Max180s = null,
                    CheckoutPercentage = null,
                    HighestCheckout = null,
                    NineDarter = null
                };

                await _playerMatchStatCollection.InsertOneAsync(stat);

                var update = Builders<Match>.Update
                    .Set(m => m.PlayerOneStatId, match.PlayerOneId == notApppearedId ? stat.Id : match.PlayerOneStatId)
                    .Set(m => m.PlayerTwoStatId, match.PlayerTwoId == notApppearedId ? stat.Id : match.PlayerTwoStatId);

                await _matchCollection.UpdateOneAsync(m => m.Id == match.Id, update);
            }
        }

        // Mobil
        public async Task SetPlayerStatAsync(string matchId, string playerId, EndMatchModel stat)
        {
            var match = await _matchCollection.Find(m => m.Id == matchId).FirstOrDefaultAsync();
            if (match == null) return;

            var playerStat = new PlayerMatchStat
            {
                Appeared = true,
                Won = stat.Won,
                SetsWon = stat.SetsWon,
                LegsWon = stat.LegsWon,
                Averages = stat.Averages,
                Max180s = stat.Max180s,
                CheckoutPercentage = stat.CheckoutPercentage,
                HighestCheckout = stat.HighestCheckout,
                NineDarter = stat.NineDarter
            };

            await _playerMatchStatCollection.InsertOneAsync(playerStat);

            bool isTournamentWon = match.RemainingPlayer == 2 && stat.Won;

            if (stat.Legs != null || stat.Sets != null)
            {
                await _usersTournamentStatService.SavePlayerTournamentStatAsync(playerId, match, stat, isTournamentWon);
            }

            UpdateDefinition<Match> update;

            if (playerId == match.PlayerOneId)
            {
                update = Builders<Match>.Update.Set(m => m.PlayerOneStatId, playerStat.Id).Set(m => m.Status, "Finished");
            }
            else if (playerId == match.PlayerTwoId)
            {
                update = Builders<Match>.Update.Set(m => m.PlayerTwoStatId, playerStat.Id).Set(m => m.Status, "Finished");
            }
            else
            {
                return;
            }

            await _matchCollection.UpdateOneAsync(m => m.Id == match.Id, update);

            if (stat.Won)
            {
                await CreateNextMatchAsync(match, playerId);            
            }

        }

        public async Task CreateNextMatchAsync(Match oldMatch, string winnerPlayerId)
        {
            int newRemainingPlayer = oldMatch.RemainingPlayer / 2;
            if (newRemainingPlayer == 1)
            {
                return;
            }
            int newRowNumber = (oldMatch.RowNumber + 1) / 2;
            bool isFirstPlayer = (oldMatch.RowNumber + 1) % 2 == 0;

            var matchHeader = await _matchHeaderService.GetByIdAsync(oldMatch.HeaderId);

            DateTime newStartDate = matchHeader!.MatchDates[matchHeader.MatchDates.IndexOf((DateTime)oldMatch.StartDate!) + 1];
            
            var isMatchExist = _matchCollection.Find(x => x.HeaderId == oldMatch.HeaderId && x.RemainingPlayer == newRemainingPlayer && x.RowNumber == newRowNumber).FirstOrDefault();

            if (isMatchExist == null)
            {
                Match addMatch = new Match()
                {
                    HeaderId = oldMatch.HeaderId,
                    PlayerOneId = isFirstPlayer ? winnerPlayerId : null,
                    PlayerTwoId = isFirstPlayer ? null : winnerPlayerId,
                    Status = "Pedding",
                    PlayerOneStatId = null,
                    PlayerTwoStatId = null,
                    StartDate = newStartDate,
                    RemainingPlayer = newRemainingPlayer,
                    RowNumber = newRowNumber
                };
                await _matchCollection.InsertOneAsync(addMatch);
            }
            else
            {
                var update = Builders<Match>.Update
                    .Set(m => m.PlayerOneId, isFirstPlayer ? winnerPlayerId : isMatchExist.PlayerOneId)
                    .Set(m => m.PlayerTwoId, isFirstPlayer ? isMatchExist.PlayerTwoId : winnerPlayerId);

                await _matchCollection.UpdateOneAsync(m => m.Id == isMatchExist.Id, update);
            }
        }

        public async Task<MatchWithPlayers?> GetMatchWithPlayersByIdAsync(string matchId)
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
                            { "from", "players_match_stats" },
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
                            { "from", "players_match_stats" },
                            { "localField", "player_two_stat_id" },
                            { "foreignField", "_id" },
                            { "as", "player_two_stat" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "match_headers" },
                            { "localField", "header_id" },
                            { "foreignField", "_id" },
                            { "as", "header" }
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
                            { "player_two_stat", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_two_stat", 0 } } } },
                            { "header", new BsonDocument { { "$arrayElemAt", new BsonArray { "$header", 0 } } } }
                        }
                    }
                },
                new BsonDocument
                {
                    { "$unset", new BsonArray { "player_one_id", "player_two_id", "player_one_stat_id", "player_two_stat_id", "header_id" } }
                }
            };

            return await _matchCollection.Aggregate<MatchWithPlayers>(pipeline).FirstOrDefaultAsync();
        }

        public async Task<List<MatchWithPlayers>?> GetUserUpcomingMatchesAsync(string userId, int? matchesCount)
        {
            var userObjectId = new ObjectId(userId);

            var matchFilter = new BsonDocument
            {
                { "$and", new BsonArray
                    {
                        new BsonDocument
                        {
                            { "$or", new BsonArray
                                {
                                    new BsonDocument { { "player_one_id", userObjectId } },
                                    new BsonDocument { { "player_two_id", userObjectId } }
                                }
                            }
                        },
                        new BsonDocument { { "status", new BsonDocument { { "$ne", "Finished" } } } },
                        new BsonDocument { { "player_one_id", new BsonDocument { { "$ne", BsonNull.Value } } } },
                        new BsonDocument { { "player_two_id", new BsonDocument { { "$ne", BsonNull.Value } } } }
                    }
                }
            };


            if (!matchesCount.HasValue)
            {
                matchFilter.Add("start_date", new BsonDocument { { "$gte", DateTime.UtcNow.AddMinutes(-10) } });
            }

            var pipeline = new List<BsonDocument>
            {
                new BsonDocument { { "$match", matchFilter } },
                new BsonDocument { { "$sort", new BsonDocument { { "start_date", 1 } } } }
            };

            if (matchesCount.HasValue)
            {
                pipeline.Add(new BsonDocument { { "$limit", matchesCount.Value } });
            }

            pipeline.AddRange(new[]
            {
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
                    { "$lookup", new BsonDocument
                        {
                            { "from", "match_headers" },
                            { "localField", "header_id" },
                            { "foreignField", "_id" },
                            { "as", "header" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$addFields", new BsonDocument
                        {
                            { "player_one", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_one_info", 0 } } } },
                            { "player_two", new BsonDocument { { "$arrayElemAt", new BsonArray { "$player_two_info", 0 } } } },
                            { "header", new BsonDocument { { "$arrayElemAt", new BsonArray { "$header", 0 } } } }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$project", new BsonDocument
                        {
                            { "player_one", 1 },
                            { "player_two", 1 },
                            { "start_date", 1 },
                            { "status", 1 },
                            { "remaining_player", 1 },
                            { "row_number", 1 },
                            { "header", 1 }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$unset", new BsonArray { "player_one_id", "player_two_id", "player_one_info", "player_two_info", "header_id" } }
                }
            });

            return await _matchCollection.Aggregate<MatchWithPlayers>(pipeline).ToListAsync();
        }
    }
}
