using Vizsga_Backend.Models.UserStatsModels;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using Vizsga_Backend.Models.SignalRModels;
using Vizsga_Backend.Models.MatchModels;

namespace VizsgaBackend.Services
{
    public interface IUsersTournamentStatService
    {
        Task<List<UsersTournamentStatWithUser>> GetTournamentsWithUsersAsync();
        Task<List<UsersTournamentStatWithUser>> GetTournamentsWithUsersNotStrictBannedAsync();
        Task<UsersTournamentStatWithUser?> GetTournamentWithUserByUserIdAsync(string userId);
        Task<UsersTournamentStat> GetTournamentByUserIdAsync(string userId);
        Task CreateAsync(UsersTournamentStat stat);
        Task<UpdateResult> SavePlayerTournamentStatAsync(string playerId, MatchModel match, EndMatchModel stat, bool tournamentWon);
    }
}
