namespace Vizsga_Backend.Models.UserModels
{
    public class ModifyUser
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? EmailAddress { get; set; }
        public string? NewPassword { get; set; }
    }
}
