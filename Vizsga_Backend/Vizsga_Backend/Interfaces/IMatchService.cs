using Vizsga_Backend.Models.MatchModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vizsga_Backend.Services
{
    public interface IMatchService
    {
        Task CreateMatchAsync(Match match);
        Task<Match> GetMatchByIdAsync(string matchId);
        Task<MatchWithPlayers?> GetMatchWithPlayersByIdAsync(string matchId);
        Task<List<MatchWithPlayers>?> GetUserUpcomingMatchesAsync(string userId, int? matchesCount);
        Task<List<MatchWithPlayers>> GetUserLastMatchesAsync(string userId, int matchesCount);
    }
}
