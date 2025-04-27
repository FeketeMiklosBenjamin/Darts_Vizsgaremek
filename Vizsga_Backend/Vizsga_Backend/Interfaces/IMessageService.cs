using Vizsga_Backend.Models.MessageModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vizsga_Backend.Services
{
    public interface IMessageService
    {
        Task<Message> GetMessageAsync(string? userId, string messageId);
        Task<List<Message>> GetUserMessagesAsync(string userId);
        Task<List<MessageWithUser>> GetAdminMessagesAsync();
        Task CreateAsync(Message message);
        Task DeleteAsync(string messageId);
    }
}
