namespace Vizsga_Backend.Models
{
    public class SessionData
    {
        public string? UserId { get; set; }
        public int UserRole { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
