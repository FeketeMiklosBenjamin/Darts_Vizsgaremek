using MongoDB.Bson.Serialization.Attributes;

namespace Vizsga_Backend.Models
{
    public class MessageSendAdmin
    {
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
    }
}
