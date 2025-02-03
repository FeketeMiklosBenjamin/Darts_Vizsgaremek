using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using VizsgaBackend.Models;

namespace VizsgaBackend.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<User>(mongoDBSettings.Value.UsersCollectionName);
        }

        public async Task<List<User>> GetAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
        }
        public async Task<User?> GetByIdAsync(string id)
        {
            return await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
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

        public async Task DeleteAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
