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
        public DateTime? TournamentStartDate { get; set; }
        public DateTime? TournamentEndDate { get; set; }

        public void ConvertToUtc()
        {
            JoinStartDate = ConvertToUtc(JoinStartDate);
            JoinEndDate = ConvertToUtc(JoinEndDate);
            TournamentStartDate = ConvertToUtc(TournamentStartDate);
            TournamentEndDate = ConvertToUtc(TournamentEndDate);
        }

        private DateTime? ConvertToUtc(DateTime? localDateTime)
        {
            if (localDateTime.HasValue)
            {
                return DateTime.SpecifyKind(localDateTime.Value, DateTimeKind.Local).ToUniversalTime();
            }
            return null;
        }
    }

}
