using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.TournamentModels;
using VizsgaBackend.Models;

namespace Vizsga_Backend.Services
{
    public class MatchHeaderService
    {
        private readonly IMongoCollection<MatchHeader> _matchHeaderCollection;
        private readonly Cloudinary _cloudinary;

        public MatchHeaderService(IOptions<MongoDBSettings> mongoDBSettings, Cloudinary cloudinary)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _matchHeaderCollection = database.GetCollection<MatchHeader>(mongoDBSettings.Value.MatchHeadersCollectionName);
            _cloudinary = cloudinary;

            Task.Run(() => EnsureTtlIndexAsync());
        }

        private async Task EnsureTtlIndexAsync()
        {
            var indexKeys = Builders<MatchHeader>.IndexKeys.Ascending(m => m.DeleteDate);
            var indexModel = new CreateIndexModel<MatchHeader>(indexKeys, new CreateIndexOptions
            {
                ExpireAfter = TimeSpan.FromSeconds(0)
            });

            await _matchHeaderCollection.Indexes.CreateOneAsync(indexModel);
        }

        public async Task<MatchHeader?> GetByIdAsync(string headerId)
        {
            return await _matchHeaderCollection.Find(x => x.Id == headerId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(MatchHeader matchHeader)
        {
            await _matchHeaderCollection.InsertOneAsync(matchHeader);
        }

        public async Task SaveBackgroundImageAsync(string matchHeaderId, string backgroundImageUrl)
        {
            var filter = Builders<MatchHeader>.Filter.Eq(u => u.Id, matchHeaderId);
            var update = Builders<MatchHeader>.Update.Set(u => u.BackroundImageUrl, backgroundImageUrl);

            await _matchHeaderCollection.UpdateOneAsync(filter, update);
        }

        // Profilkép törlése a Cloudinary-ból
        public async Task DeleteBackgroundImageAsync(MatchHeader header)
        {
            if (header != null && !string.IsNullOrEmpty(header.BackroundImageUrl) && header.BackroundImageUrl != "https://res.cloudinary.com/dvikunqov/image/upload/v1743843175/darts_background_pictures/ftrmvy0bpxjgoxj5tmzm.jpg")
            {
                // Kép publicId-ját kinyerjük a profilkép URL-jéből
                var publicId = ExtractPublicIdFromUrl(header.BackroundImageUrl);

                var deleteParams = new DeletionParams(publicId);
                var deletionResult = await _cloudinary.DestroyAsync(deleteParams);
            }
        }

        // PublicId kinyerése a Cloudinary URL-ből
        private string ExtractPublicIdFromUrl(string url)
        {
            var regex = new Regex(@"image\/upload\/v\d+\/(.*?)(?=\.)");
            var match = regex.Match(url);

            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        public async Task DeleteMatchHeaderAsync(string matchHeaderId)
        {
            await _matchHeaderCollection.DeleteOneAsync(x => x.Id == matchHeaderId);
        }

        public async Task SetDrawedAsync(string matchHeaderId)
        {
            var filter = Builders<MatchHeader>.Filter.Eq(u => u.Id, matchHeaderId);
            var update = Builders<MatchHeader>.Update.Set(u => u.IsDrawed, true);

            await _matchHeaderCollection.UpdateOneAsync(filter, update);
        }

        public async Task<List<MatchHeader>> GetAllDrawedTournamentAsync()
        {
            return await _matchHeaderCollection.Find(x => x.IsDrawed == true).ToListAsync();
        }

        public async Task<MatchHeaderWithMatches?> GetTournamentWithMatchesAsync(string matchHeaderId)
        {
            var pipeline = new[]
            {
                new BsonDocument
                {
                    { "$match", new BsonDocument { { "_id", new ObjectId(matchHeaderId) } } }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "matches" },
                            { "localField", "_id" },
                            { "foreignField", "header_id" },
                            { "as", "matches" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "users" },
                            { "localField", "matches.player_one_id" },
                            { "foreignField", "_id" },
                            { "as", "players_one" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "users" },
                            { "localField", "matches.player_two_id" },
                            { "foreignField", "_id" },
                            { "as", "players_two" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "player_match_stats" },
                            { "localField", "matches.player_one_id" },
                            { "foreignField", "player_id" },
                            { "as", "player_one_stats" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "player_match_stats" },
                            { "localField", "matches.player_two_id" },
                            { "foreignField", "player_id" },
                            { "as", "player_two_stats" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$addFields", new BsonDocument
                        {
                            { "matches", new BsonDocument
                                {
                                    { "$map", new BsonDocument
                                        {
                                            { "input", "$matches" },
                                            { "as", "match" },
                                            { "in", new BsonDocument
                                                {
                                                    { "_id", "$$match._id" },
                                                    { "status", "$$match.status" },
                                                    { "start_date", "$$match.start_date" },
                                                    { "remaining_player", "$$match.remaining_player" },
                                                    { "row_number", "$$match.row_number" },

                                                    { "player_one", new BsonDocument
                                                        {
                                                            { "$first", new BsonDocument
                                                                {
                                                                    { "$filter", new BsonDocument
                                                                        {
                                                                            { "input", "$players_one" },
                                                                            { "as", "user" },
                                                                            { "cond", new BsonDocument
                                                                                {
                                                                                    { "$eq", new BsonArray
                                                                                        { "$$user._id", "$$match.player_one_id" }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    },

                                                    { "player_two", new BsonDocument
                                                        {
                                                            { "$first", new BsonDocument
                                                                {
                                                                    { "$filter", new BsonDocument
                                                                        {
                                                                            { "input", "$players_two" },
                                                                            { "as", "user" },
                                                                            { "cond", new BsonDocument
                                                                                {
                                                                                    { "$eq", new BsonArray
                                                                                        { "$$user._id", "$$match.player_two_id" }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    },

                                                    { "player_one_stat", new BsonDocument
                                                        {
                                                            { "$first", new BsonDocument
                                                                {
                                                                    { "$filter", new BsonDocument
                                                                        {
                                                                            { "input", "$player_one_stats" },
                                                                            { "as", "stat" },
                                                                            { "cond", new BsonDocument
                                                                                {
                                                                                    { "$eq", new BsonArray
                                                                                        { "$$stat.player_id", "$$match.player_one_id" }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    },

                                                    { "player_two_stat", new BsonDocument
                                                        {
                                                            { "$first", new BsonDocument
                                                                {
                                                                    { "$filter", new BsonDocument
                                                                        {
                                                                            { "input", "$player_two_stats" },
                                                                            { "as", "stat" },
                                                                            { "cond", new BsonDocument
                                                                                {
                                                                                    { "$eq", new BsonArray
                                                                                        { "$$stat.player_id", "$$match.player_two_id" }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },

                new BsonDocument { { "$unset", new BsonArray { "players_one", "players_two", "player_one_stats", "player_two_stats" } } }
            };

            return await _matchHeaderCollection.Aggregate<MatchHeaderWithMatches>(pipeline).FirstOrDefaultAsync();
        }

        public async Task<List<MatchHeader>> GetAllFriendlyMatchAsync()
        {
            return await _matchHeaderCollection.Find(x => x.TournamentStartDate == null && x.TournamentEndDate == null && x.DeleteDate != null).ToListAsync();
        }

        public string ValidateFriendlyMatchDatas(FriendlyGameCreate datas)
        {
            if (datas.SetsCount != null && datas.SetsCount <= 0)
            {
                return "A set darabszáma nem lehet nulla vagy negatív!";
            }
            if (datas.LegsCount < 2)
            {
                return "A legek számának minimum 2-nek kell lennie!";
            }
            if (datas.StartingPoint != 701 && datas.StartingPoint != 501 && datas.StartingPoint != 301)
            {
                return "A kézdő pont csak 301, 501 és 701-es értéket vehet fel!";
            }
            return "";
        }

        public async Task SetDeleteDateToNullAsync(string matchHeaderId)
        {
            var filter = Builders<MatchHeader>.Filter.Eq(u => u.Id, matchHeaderId);
            var update = Builders<MatchHeader>.Update.Set(u => u.DeleteDate, null);

            await _matchHeaderCollection.UpdateOneAsync(filter, update);
        }
    }
}
