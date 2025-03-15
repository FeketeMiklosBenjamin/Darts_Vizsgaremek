using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Vizsga_Backend.Models.TournamentModels
{
    public class AnnouncedTournament
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("header_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string HeaderId { get; set; } = string.Empty;

        [BsonElement("join_start_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime JoinStartDate { get; set; }

        [BsonElement("join_end_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime JoinEndDate { get; set; }

        [BsonElement("max_player_join")]
        public int MaxPlayerJoin {  get; set; }
    }
}
