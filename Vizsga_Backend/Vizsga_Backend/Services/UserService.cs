using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using StackExchange.Redis;
using System.Net;
using System.Text.RegularExpressions;
using Vizsga_Backend.Models.UserModels;
using VizsgaBackend.Models;

namespace VizsgaBackend.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly Cloudinary _cloudinary;

        public UserService(IOptions<MongoDBSettings> mongoDBSettings, Cloudinary cloudinary)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<User>(mongoDBSettings.Value.UsersCollectionName);
            _cloudinary = cloudinary;
        }



        // GET végpont:

        public async Task<List<User>> GetNotStrictBannedAsync()
        {
            return await _usersCollection.Find(x => x.StrictBan == false && x.Role == 1).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        // Alap végpontokhoz vagy ellenőrzésekhez:



        public async Task CreateAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task SetUserBanAsync(string userId, bool strictBan, DateTime? bannedUntil)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            UpdateDefinition<User> update;
            if (bannedUntil != null)
            {
                update = Builders<User>.Update.Set(u => u.StrictBan, strictBan)
                    .Set(u => u.BannedUntil, bannedUntil)
                    .Unset(u => u.RefreshTokens);
            }
            else
            {
                update = Builders<User>.Update.Set(u => u.StrictBan, strictBan)
                    .Set(u => u.BannedUntil, bannedUntil);
            }

            await _usersCollection.UpdateOneAsync(filter, update);
        }

        public async Task<bool> IsEmailTakenAsync(string email, string? userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.EmailAddress, email);
            var user = await _usersCollection.Find(filter).FirstOrDefaultAsync();
            if (userId == null)
            {
                return user != null;
            }
            return user != null && user.Id != userId;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var regex = new Regex(emailPattern);

            return regex.IsMatch(email);
        }

        public async Task<User> GetUserByEmailAsync(string emailAddress)
        {
            var user = await _usersCollection
                .Find(u => u.EmailAddress == emailAddress)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<User> filter, UpdateDefinition<User> update)
        {
            return await _usersCollection.UpdateOneAsync(filter, update);
        }



        // Kép feltöltésekhez:



        public async Task SaveProfilePictureAsync(string userId, string profilePicture)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Set(u => u.ProfilePicture, profilePicture);

            await _usersCollection.UpdateOneAsync(filter, update);
        }

        // Profilkép törlése a Cloudinary-ból
        public async Task DeleteProfilePictureAsync(string userId)
        {
            var user = await _usersCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();

            if (user != null && !string.IsNullOrEmpty(user.ProfilePicture) && user.ProfilePicture != "https://res.cloudinary.com/dvikunqov/image/upload/v1740128607/darts_profile_pictures/fvlownxvkn4etrkvfutl.jpg")
            {
                // Kép publicId-ját kinyerjük a profilkép URL-jéből
                var publicId = ExtractPublicIdFromUrl(user.ProfilePicture);

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



        // Token kezeléshez:



        // Refresh token mentése
        public async Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Push(u => u.RefreshTokens, refreshToken)
                                               .Set(u => u.LastLoginDate, DateTime.UtcNow);

            await _usersCollection.UpdateOneAsync(filter, update);
        }

        public async Task RefreshLastLoginDateAsync(string userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Set(u => u.LastLoginDate, DateTime.UtcNow);

            await _usersCollection.UpdateOneAsync(filter, update);
        }

        // Refresh token validálása
        public async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var filter = Builders<User>.Filter.AnyEq(u => u.RefreshTokens, refreshToken);
            var user = await _usersCollection.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task DeleteRefreshTokenAsync(string userId, string refreshToken)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Pull(u => u.RefreshTokens, refreshToken);

            await _usersCollection.UpdateOneAsync(filter, update);
        }

    }

}
