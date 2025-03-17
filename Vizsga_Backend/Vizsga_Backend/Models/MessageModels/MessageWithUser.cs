using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Vizsga_Backend.Models.UserModels;

namespace Vizsga_Backend.Models.MessageModels
{
    public class MessageWithUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("from_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? FromId { get; set; } = string.Empty;

        [BsonElement("to_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ToId { get; set; } = string.Empty;

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("text")]
        public string Text { get; set; } = string.Empty;

        [BsonElement("send_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime SendDate { get; set; }

        [BsonElement("user")]
        public User? User { get; set; }
    }
}
