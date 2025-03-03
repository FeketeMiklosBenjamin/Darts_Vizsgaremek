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

        [BsonElement("profile_picture")]
        public string ProfilePicture { get; set; } = string.Empty;

        [BsonElement("role")]
        public int Role { get; set; }

        [BsonElement("register_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime RegisterDate { get; set; }

        // Több refresh tokent tároló lista
        [BsonElement("refresh_tokens")]
        public List<string> RefreshTokens { get; set; } = new List<string>();

        // A refresh tokenek lejárati dátumainak listája
        [BsonElement("refresh_token_expiries")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public List<DateTime?> RefreshTokenExpiries { get; set; } = new List<DateTime?>();

        [BsonElement("last_login_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastLoginDate { get; set; }

        [BsonElement("strict_ban")]
        public bool StrictBan { get; set; }

        [BsonElement("banned_until")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? BannedUntil { get; set; }
    }
}
