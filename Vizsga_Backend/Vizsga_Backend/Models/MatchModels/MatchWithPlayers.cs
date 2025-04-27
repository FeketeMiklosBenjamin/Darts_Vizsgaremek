using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Vizsga_Backend.Models.UserModels;
using Vizsga_Backend.Models.UserStatsModels;

namespace Vizsga_Backend.Models.MatchModels
{
    public class MatchWithPlayers
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("header")]
        public MatchHeader? Header { get; set; }

        [BsonElement("player_one")]
        public User? PlayerOne { get; set; }

        [BsonElement("player_two")]
        public User? PlayerTwo { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("player_one_stat")]
        public PlayerMatchStat? PlayerOneStat { get; set; }

        [BsonElement("player_two_stat")]
        public PlayerMatchStat? PlayerTwoStat { get; set; }

        [BsonElement("start_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? StartDate { get; set; }

        [BsonElement("remaining_player")]
        public int RemainingPlayer { get; set; }

        [BsonElement("row_number")]
        public int RowNumber { get; set; }
    }
}
