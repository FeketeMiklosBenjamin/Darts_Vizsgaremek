using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Vizsga_Backend.Models;
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
    }
}
