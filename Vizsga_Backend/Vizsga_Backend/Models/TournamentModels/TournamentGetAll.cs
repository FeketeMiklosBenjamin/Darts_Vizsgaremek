using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.UserModels;
using System.Text.Json.Serialization;

namespace Vizsga_Backend.Models.TournamentModels
{
    public class TournamentGetAll
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("header_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string HeaderId { get; set; } = string.Empty;

        [BsonElement("required_level")]
        public int RequiredLevel { get; set; }

        [BsonElement("max_level")]
        public int? MaxLevel { get; set; }

        [BsonElement("join_start_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime JoinStartDate { get; set; }

        [BsonElement("join_end_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime JoinEndDate { get; set; }

        [BsonElement("max_player_join")]
        public int MaxPlayerJoin { get; set; }

        [BsonElement("match_header")]
        public MatchHeader? MatchHeader { get; set; }

        [BsonElement("registered_players")]
        public List<User> RegisteredPlayers { get; set; } = new List<User>();

        [BsonElement("players_tournaments")]
        [JsonIgnore]
        public List<PlayerTournament> PlayerTournaments { get; set; } = new List<PlayerTournament>();
    }

}
