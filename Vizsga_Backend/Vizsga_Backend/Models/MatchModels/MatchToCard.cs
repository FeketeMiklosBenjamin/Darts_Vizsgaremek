using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Vizsga_Backend.Models.MatchModels
{
    public class MatchToCard
    {
        public string MatchId { get; set; } = string.Empty;
        public string HeaderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string playerOneName { get; set; } = string.Empty;
        public string playerTwoName { get; set; } = string.Empty;
        public int? PlayerOneResult { get; set; }
        public int? PlayerTwoResult { get; set; }
        public bool? Won { get; set; }

        public MatchToCard(string matchId, string headerId, string status, string startDate, string playerOneName, string playerTwoName, int? playerOneResult, int? playerTwoResult, bool? won)
        {
            MatchId = matchId;
            HeaderId = headerId;
            Status = status;
            StartDate = startDate;
            this.playerOneName = playerOneName;
            this.playerTwoName = playerTwoName;
            PlayerOneResult = playerOneResult;
            PlayerTwoResult = playerTwoResult;
            Won = won;
        }
    }
}
