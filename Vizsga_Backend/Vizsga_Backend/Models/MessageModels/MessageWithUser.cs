using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Vizsga_Backend.Models.UserModels;

namespace Vizsga_Backend.Models.MessageModels
{
    public class MessageWithUser
    {
        public string Id { get; set; } = string.Empty;
        public string? FromId { get; set; } = string.Empty;
        public string? ToId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
        public User? User { get; set; }
    }
}
