namespace Vizsga_Backend.Models.TournamentModels
{
    public class TournamentCreate
    {
        public string? Name { get; set; }
        public int? SetsCount { get; set; }
        public int? LegsCount { get; set; }
        public int? StartingPoint { get; set; }
        public string? Password { get; set; }
        public string? Level { get; set; }
        public int? MaxPlayerJoin { get; set; }
        public DateTime? JoinStartDate { get; set; }
        public DateTime? JoinEndDate { get; set; }
        public List<DateTime> MatchDates { get; set; } = new List<DateTime>();
    }


}
