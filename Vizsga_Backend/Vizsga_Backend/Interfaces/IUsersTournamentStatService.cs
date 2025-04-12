using Vizsga_Backend.Models.UserStatsModels;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VizsgaBackend.Services
{
    public interface IUsersTournamentStatService
    {
        Task<List<UsersTournamentStatWithUser>> GetTournamentsWithUsersAsync();
        Task<List<UsersTournamentStatWithUser>> GetTournamentsWithUsersNotStrictBannedAsync();
        Task<UsersTournamentStatWithUser?> GetTournamentWithUserByUserIdAsync(string userId);
        Task<UsersTournamentStat> GetTournamentByUserIdAsync(string userId);
        Task CreateAsync(UsersTournamentStat stat);
        Task<UpdateResult> UpdateOneAsync(FilterDefinition<UsersTournamentStat> filter, UpdateDefinition<UsersTournamentStat> update);
    }
}
