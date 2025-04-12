using CloudinaryDotNet.Actions;
using MongoDB.Driver;
using Vizsga_Backend.Models.UserModels;
using System.Threading.Tasks;

namespace Vizsga_Backend.Interfaces
{
    public interface IUserService
    {
        // GET végpontok:
        Task<List<User>> GetNotStrictBannedAsync();
        Task<User?> GetByIdAsync(string id);

        // Alap végpontokhoz vagy ellenőrzésekhez:
        Task CreateAsync(User user);
        Task SetUserBanAsync(string userId, bool strictBan, DateTime? bannedUntil);
        Task<bool> IsEmailTakenAsync(string email, string? userId);
        bool IsValidEmail(string email);
        Task<User?> GetUserByEmailAsync(string emailAddress);
        Task<UpdateResult> UpdateOneAsync(FilterDefinition<User> filter, UpdateDefinition<User> update);

        // Kép feltöltésekhez:
        Task SaveProfilePictureAsync(string userId, string profilePicture);
        Task DeleteProfilePictureAsync(string userId);

        // Token kezeléshez:
        Task SaveRefreshTokenAsync(string userId, string refreshToken);
        Task RefreshLastLoginDateAsync(string userId);
        Task<User?> ValidateRefreshTokenAsync(string refreshToken);
        Task DeleteRefreshTokenAsync(string userId, string refreshToken);
    }
}
