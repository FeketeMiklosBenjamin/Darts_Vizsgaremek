using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Vizsga_Backend.Models.MatchModels
{
    public class MatchHeader
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("created_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? CreatedDate { get; set; }

        [BsonElement("sets_count")]
        public int SetsCount { get; set; }

        [BsonElement("legs_count")]
        public int LegsCount { get; set; }

        [BsonElement("starting_point")]
        public int StartingPoint { get; set; }

        [BsonElement("join_password")]
        public string JoinPassword { get; set; } = string.Empty;

        [BsonElement("background_image_url")]
        public string BackroundImageUrl { get; set; } = string.Empty;

        [BsonElement("tournament_start_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? TournamentStartDate { get; set; }

        [BsonElement("tournament_end_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? TournamentEndDate { get; set; }
    }
}
