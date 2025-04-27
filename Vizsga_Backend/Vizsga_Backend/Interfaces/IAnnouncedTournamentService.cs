using Vizsga_Backend.Models.TournamentModels;
using VizsgaBackend.Models;

namespace Vizsga_Backend.Services
{
    public interface IAnnouncedTournamentService
    {
        Task<List<TournamentGetAll>> GetAnnouncedTournamentsWithPlayersAsync();
        Task<AnnouncedTournament?> GetAnnouncedTournamentByIdAsync(string tournamentId);
        Task<bool> DoesPlayerJoinedThisTournamentAsync(string announcedTournamentId, string userId);
        Task<int> JoinedPlayerToTournamentCountAsync(string announcedTournamentId);
        Task<List<PlayerTournament>> GetJoinedPlayersAsync(string announcedTournamentId);
        string ValidateCreateDatas(TournamentCreate datas);
        Task CreateTournamentAsync(AnnouncedTournament announcedTournament);
        Task CreateRegisterAsync(PlayerTournament playerTournament);
        Task DeleteAllRegisterAndTournamentAsync(string announcedTournamentId);
    }
}
