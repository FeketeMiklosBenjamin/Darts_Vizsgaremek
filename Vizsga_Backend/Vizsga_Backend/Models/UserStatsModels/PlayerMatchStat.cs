using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Vizsga_Backend.Models.UserStatsModels
{
    public class PlayerMatchStat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("appeared")]
        public bool Appeared { get; set; }

        [BsonElement("won")]
        public bool Won { get; set; }

        [BsonElement("sets_won")]
        public int? SetsWon { get; set; }

        [BsonElement("legs_won")]
        public int? LegsWon { get; set; }

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
    }
}
