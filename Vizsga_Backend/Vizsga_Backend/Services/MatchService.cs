using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Vizsga_Backend.Models.MatchModels;
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
    }
}
