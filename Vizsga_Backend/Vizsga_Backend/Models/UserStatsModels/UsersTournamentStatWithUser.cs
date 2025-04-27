using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Vizsga_Backend.Models.UserModels;

namespace Vizsga_Backend.Models.UserStatsModels
{
    public class UsersTournamentStatWithUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("matches")]
        public int? Matches { get; set; }

        [BsonElement("matches_won")]
        public int? MatchesWon { get; set; }

        [BsonElement("sets")]
        public int? Sets { get; set; }

        [BsonElement("sets_won")]
        public int? SetsWon { get; set; }

        [BsonElement("legs")]
        public int? Legs { get; set; }

        [BsonElement("legs_won")]
        public int? LegsWon { get; set; }

        [BsonElement("tournaments_won")]
        public int? TournamentsWon { get; set; }

        [BsonElement("darts_points")]
        public int? DartsPoints { get; set; }

        [BsonElement("level")]
        public string Level { get; set; } = string.Empty;

        [BsonElement("averages")]
        public double? Averages { get; set; }

        [BsonElement("max180s")]
        public int? Max180s { get; set; }

        [BsonElement("checkout_percentage")]
        public double? CheckoutPercentage { get; set; }

        [BsonElement("highest_checkout")]
        public int? HighestCheckout { get; set; }

        [BsonElement("nine_darter")]
        public int? NineDarter { get; set; }

        [BsonElement("user")]
        public User? User { get; set; }

    }
}
