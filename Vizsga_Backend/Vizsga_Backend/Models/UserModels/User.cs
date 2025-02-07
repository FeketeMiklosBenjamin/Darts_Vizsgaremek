using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using VizsgaBackend.Models;

namespace Vizsga_Backend.Models.UserModels
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

        // Új mező a refresh token tárolásához
        [BsonElement("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        // Esetleg a refresh token lejárati dátuma (ha szükséges)
        [BsonElement("refresh_token_expiry")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? RefreshTokenExpiry { get; set; }

        [BsonElement("last_login_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastLoginDate { get; set; }
    }

}
