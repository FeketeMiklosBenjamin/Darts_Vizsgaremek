using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Vizsga_Backend.Models.UserModels;

namespace Vizsga_Backend.Models.UserStatsModels
{
    public class UsersTournamentStatWithUser
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int? Matches { get; set; }
        public int? MatchesWon { get; set; }
        public int? Sets { get; set; }
        public int? SetsWon { get; set; }
        public int? Legs { get; set; }
        public int? LegsWon { get; set; }
        public int? TournamentsWon { get; set; }
        public int? DartsPoints { get; set; }
        public double? Averages { get; set; }
        public int? Max180s { get; set; }
        public double? CheckoutPercentage { get; set; }
        public int? HighestCheckout { get; set; }
        public int? NineDarter { get; set; }
        public User? User { get; set; }

    }
}
