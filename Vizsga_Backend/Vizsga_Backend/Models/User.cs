using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace VizsgaBackend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        [BsonElement("email_address")] 
        public string EmailAddress { get; set; } = string.Empty;

        [BsonElement("role")]
        public int Role { get; set; }

        [BsonElement("register_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime RegisterDate { get; set; }
    }
}
