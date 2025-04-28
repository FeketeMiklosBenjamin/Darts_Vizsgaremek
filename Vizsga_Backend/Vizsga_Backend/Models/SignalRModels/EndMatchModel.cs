using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Vizsga_Backend.Models.SignalRModels
{
    public class EndMatchModel
    {
        public bool Won { get; set; }

        public int? Sets { get; set; }

        public int? SetsWon { get; set; }

        public int? Legs { get; set; }

        public int? LegsWon { get; set; }

        public double? Averages { get; set; }

        public int? Max180s { get; set; }

        public double? CheckoutPercentage { get; set; }

        public int? HighestCheckout { get; set; }

        public int? NineDarter { get; set; }
    }
}
