using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Vizsga_Backend.Models.MatchModels;
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
            if (header != null && !string.IsNullOrEmpty(header.BackroundImageUrl) && header.BackroundImageUrl != "https://res.cloudinary.com/dvikunqov/image/upload/v1740128607/darts_profile_pictures/fvlownxvkn4etrkvfutl.jpg")
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
    }
}
