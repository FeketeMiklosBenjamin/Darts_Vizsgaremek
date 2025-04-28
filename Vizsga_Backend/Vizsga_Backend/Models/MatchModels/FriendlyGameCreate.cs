namespace Vizsga_Backend.Models.MatchModels
{
    public class FriendlyGameCreate
    {
        public int? SetsCount { get; set; }
        public int LegsCount { get; set; } = 0;
        public int StartingPoint { get; set; } = 0;
        public string? JoinPassword { get; set; }
        public bool LevelLocked { get; set; } = false;
    }
}
