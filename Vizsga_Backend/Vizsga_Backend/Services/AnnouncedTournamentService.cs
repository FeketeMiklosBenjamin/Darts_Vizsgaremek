using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Vizsga_Backend.Models.TournamentModels;
using VizsgaBackend.Models;

namespace Vizsga_Backend.Services
{
    public class AnnouncedTournamentService
    {
        private readonly IMongoCollection<AnnouncedTournament> _announcedTournamentCollection;
        private readonly IMongoCollection<PlayerTournament> _playerTournamentCollection;

        public AnnouncedTournamentService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _announcedTournamentCollection = database.GetCollection<AnnouncedTournament>(mongoDBSettings.Value.AnnouncedTournamentsCollectionName);
            _playerTournamentCollection = database.GetCollection<PlayerTournament>(mongoDBSettings.Value.PlayersTournamentsCollectionName);
        }

        public async Task<List<TournamentGetAll>> GetAnnouncedTournamentsWithPlayersAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "match_headers" },
                            { "localField", "header_id" },
                            { "foreignField", "_id" },
                            { "as", "match_header" }
                        }
                    }
                },
                new BsonDocument { { "$unwind", new BsonDocument { { "path", "$match_header" }, { "preserveNullAndEmptyArrays", true } } } },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "players-tournaments" },
                            { "localField", "_id" },
                            { "foreignField", "announced_tournament_id" },
                            { "as", "players_tournaments" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "$lookup", new BsonDocument
                        {
                            { "from", "users" },
                            { "localField", "players_tournaments.user_id" },
                            { "foreignField", "_id" },
                            { "as", "registered_players" }
                        }
                    }
                }
            };

            var results = await _announcedTournamentCollection.Aggregate<TournamentGetAll>(pipeline).ToListAsync();
            return results;
        }

        public async Task<AnnouncedTournament?> GetAnnouncedTournamentByIdAsync(string tournamentId)
        {
            return await _announcedTournamentCollection.Find(x => x.Id == tournamentId).FirstOrDefaultAsync();
        }

        public async Task<bool> DoesPlayerJoinedThisTournamentAsync(string announcedTournamentId, string userId)
        {
            var filter = Builders<PlayerTournament>.Filter.And(
                Builders<PlayerTournament>.Filter.Eq(pt => pt.AnnoucedTournamentId, announcedTournamentId),
                Builders<PlayerTournament>.Filter.Eq(pt => pt.UserId, userId)
            );

            var exists = await _playerTournamentCollection.Find(filter).AnyAsync();
            return exists;
        }

        public async Task<int> JoinedPlayerToTournamentCountAsync(string announcedTournamentId)
        {
            var filter = Builders<PlayerTournament>.Filter.Eq(pt => pt.AnnoucedTournamentId, announcedTournamentId);
            var count = await _playerTournamentCollection.CountDocumentsAsync(filter);
            return (int)count;
        }

        public async Task<List<PlayerTournament>> GetJoinedPlayersAsync(string announcedTournamentId)
        {
            var players = await _playerTournamentCollection.FindAsync(x => x.AnnoucedTournamentId == announcedTournamentId);
            return players.ToList();
        }


        public string ValidateCreateDatas(TournamentCreate datas)
        {
            if (String.IsNullOrWhiteSpace(datas.Name))
            {
                return "A cím mező nem lehet űres!";
            }

            if (datas.LegsCount == null)
            {
                return "A legek számát kötelező megadni!";
            }

            if (datas.LegsCount < 2)
            {
                return "A legek számának minimum 2-nek kell lennie!";
            }

            if (datas.StartingPoint == null || (datas.StartingPoint != 701 && datas.StartingPoint != 501 && datas.StartingPoint != 301))
            {
                return "A kézdő pont csak 301, 501 és 701-es értéket vehet fel!";
            }

            if (String.IsNullOrWhiteSpace(datas.Password) || datas.Password.Length < 8)
            {
                return "A jelszónak minimum 8 karakter hosszúnak kell lennie!";
            }

            if (datas.Level != "Amatőr" && datas.Level != "Haladó" && datas.Level != "Profi" && datas.Level != "Bajnok")
            {
                return "A szint nem megfelelő!";
            }

            if (datas.MaxPlayerJoin == null || datas.MaxPlayerJoin != 16 && datas.MaxPlayerJoin != 8 && datas.MaxPlayerJoin != 4)
            {
                return "A jelentkezők száma csak 16, 8 és 4 fő lehet!";
            }

            int requiredDatesCount = 2;

            switch (datas.MaxPlayerJoin)
            {
                case 16:
                    requiredDatesCount = 4;
                    break;
                case 8:
                    requiredDatesCount = 3;
                    break;
                default:
                    requiredDatesCount = 2;
                    break;
            }

            if (datas.MatchDates.Count() != requiredDatesCount)
            {
                return $"A megadott dátumok száma ({datas.MatchDates.Count()}) nem egyenlő a megadott jelentkező számnál elvárt darabszámmal ({requiredDatesCount}) !";
            }

            datas.MatchDates.Sort();

            DateTime tournamentStartDate = datas.MatchDates[0];
            DateTime tournamentEndDats = datas.MatchDates[datas.MatchDates.Count() - 1];

            if (datas.JoinStartDate == null || datas.JoinEndDate == null)
            {
                return "A jelentezés kezdeti és lezárási idejét kötelező megadni!";
            }

            if (datas.JoinEndDate <= datas.JoinStartDate || tournamentStartDate <= datas.JoinEndDate)
            {
                return "A megadott időpontoknál logikai ellentmondás található!";
            }

            if (datas.JoinStartDate < DateTime.UtcNow)
            {
                return "A megadott időpontok nem lehetnek régebbiek a mostani időnél!";
            }


            return "";
        }

        public async Task CreateTournamentAsync(AnnouncedTournament announcedTournament)
        {
            await _announcedTournamentCollection.InsertOneAsync(announcedTournament);
        }

        public async Task CreateRegisterAsync(PlayerTournament playerTournament)
        {
            await _playerTournamentCollection.InsertOneAsync(playerTournament);
        }
        public async Task DeleteAllRegisterAndTournamentAsync(string announcedTournamentId)
        {
            var filter = Builders<PlayerTournament>.Filter.Eq(x => x.AnnoucedTournamentId, announcedTournamentId);
            await _playerTournamentCollection.DeleteManyAsync(filter);
            await _announcedTournamentCollection.DeleteOneAsync(x => x.Id == announcedTournamentId);
        }
    }
}
