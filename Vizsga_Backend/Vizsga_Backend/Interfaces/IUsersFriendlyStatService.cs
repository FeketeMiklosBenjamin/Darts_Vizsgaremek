using Vizsga_Backend.Models.UserStatsModels;
using MongoDB.Driver;
using System.Threading.Tasks;
using Vizsga_Backend.Models.SignalRModels;

namespace VizsgaBackend.Services
{
    public interface IUsersFriendlyStatService
    {
        Task<UsersFriendlyStat> GetByUserIdAsync(string userId);
        Task CreateAsync(UsersFriendlyStat stat);
        Task<UpdateResult> SavePlayerStat(string userId, EndMatchModel stats);
    }
}
