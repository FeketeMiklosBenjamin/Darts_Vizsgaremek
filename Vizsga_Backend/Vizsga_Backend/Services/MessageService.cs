using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Vizsga_Backend.Models.MessageModels;
using VizsgaBackend.Models;

namespace Vizsga_Backend.Services
{
    public class MessageService
    {
        private readonly IMongoCollection<Message> _messagesCollection;

        public MessageService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _messagesCollection = database.GetCollection<Message>(mongoDBSettings.Value.MessagesCollectionName);
        }

        public async Task<Message> GetMessageAsync(string? userId, string messageId)
        {
            return await _messagesCollection.Find(x => x.ToId == userId && x.Id == messageId).FirstOrDefaultAsync();
        }

        public async Task<List<Message>> GetUserMessages(string userId)
        {
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument
                {
                    { "to_id", new ObjectId(userId) }
                })
            };

            var result = await _messagesCollection.Aggregate<Message>(pipeline).ToListAsync();
            return result;
        }

        public async Task<List<MessageWithUser>> GetAdminMessages()
        {
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument
                {
                    { "to_id", BsonNull.Value }
                }),

                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "users" },
                    { "localField", "from_id" },
                    { "foreignField", "_id" },
                    { "as", "user" }
                }),
                new BsonDocument("$unwind", "$user")
            };

            var result = await _messagesCollection.Aggregate<MessageWithUser>(pipeline).ToListAsync();
            return result;
        }

        public async Task CreateAsync(Message message)
        {
            await _messagesCollection.InsertOneAsync(message);
        }

        public async Task DeleteAsync(string messageId)
        {
            await _messagesCollection.DeleteOneAsync(x => x.Id == messageId);
        }
    }
}
