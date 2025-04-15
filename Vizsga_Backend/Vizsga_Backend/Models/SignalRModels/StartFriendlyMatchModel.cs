namespace Vizsga_Backend.Models.SignalRModels
{
    public class StartFriendlyMatchModel
    {
        public string StartingPlayer { get; set; }
        public int SetCount { get; set; }
        public int LegCount { get; set; }
        public int StartingPoint { get; set; }
        public string? PlayerOneName { get; set; }
        public string? PlayerTwoName { get; set; }
    }
}
