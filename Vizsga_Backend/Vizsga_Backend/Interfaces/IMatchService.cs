using Vizsga_Backend.Models.MatchModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vizsga_Backend.Models.SignalRModels;

namespace Vizsga_Backend.Services
{
    public interface IMatchService
    {
        Task CreateMatchAsync(Match match);
        Task<Match> GetMatchByIdAsync(string matchId);
        Task<MatchWithPlayers?> GetMatchWithPlayersByIdAsync(string matchId);
        Task<List<MatchWithPlayers>?> GetUserUpcomingMatchesAsync(string userId, int? matchesCount);
        //Task<List<MatchWithPlayers>> GetUserLastMatchesAsync(string userId, int matchesCount);
        Task SetAllPlayerStatNotAppearedAsync(string matchId, string? notAppearedId);
        Task SetPlayerStatAsync(string matchId, string playerId, EndMatchModel stat);
        Task CreateNextMatchAsync(Match oldMatch, string winnerPlayerId);
    }
}
