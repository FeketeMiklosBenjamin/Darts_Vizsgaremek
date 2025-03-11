using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Vizsga_Backend.Models.TournamentModels
{
    public class PlayerTournament
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("announced_tournament_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AnnoucedTournamentId { get; set; } = string.Empty;

        [BsonElement("joined_number")]
        public int JoinedNumber { get; set; }
    }
}
