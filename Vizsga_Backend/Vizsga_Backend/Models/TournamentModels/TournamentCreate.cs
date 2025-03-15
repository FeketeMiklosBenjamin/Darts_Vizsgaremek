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

        public void ConvertToUtc()
        {
            JoinStartDate = ConvertToUtc(JoinStartDate);
            JoinEndDate = ConvertToUtc(JoinEndDate);

            // Az összes MatchDate konvertálása UTC-re
            for (int i = 0; i < MatchDates.Count; i++)
            {
                MatchDates[i] = DateTime.SpecifyKind(MatchDates[i], DateTimeKind.Local).ToUniversalTime();
            }
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
