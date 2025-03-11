using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Vizsga_Backend.Models.MatchModels
{
    public class Match
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("header_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string HeaderId { get; set; } = string.Empty;

        [BsonElement("player_one_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlayerOneId { get; set; } = string.Empty;

        [BsonElement("player_two_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlayerTwoId { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("player_one_stat_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlayerOneStatId { get; set; } = string.Empty;

        [BsonElement("player_two_stat_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlayerTwoStatId { get; set; } = string.Empty;

        [BsonElement("start_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? StartDate { get; set; }

        [BsonElement("remaining_player")]
        public int? RemainingPlayer { get; set; }

        [BsonElement("row_number")]
        public int? RowNumber { get; set; }
    }
}
