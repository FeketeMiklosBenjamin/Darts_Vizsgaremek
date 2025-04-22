using Vizsga_Backend.Models.MatchModels;

namespace Vizsga_Backend.Services
{
    public interface IMatchHeaderService
    {
        Task<MatchHeader?> GetByIdAsync(string headerId);
        Task CreateAsync(MatchHeader matchHeader);
        Task SaveBackgroundImageAsync(string matchHeaderId, string backgroundImageUrl);
        Task DeleteBackgroundImageAsync(MatchHeader header);
        Task DeleteMatchHeaderAsync(string matchHeaderId);
        Task SetDrawedAsync(string matchHeaderId);
        Task<List<MatchHeader>> GetAllDrawedTournamentAsync();
        Task<MatchHeaderWithMatches?> GetTournamentWithMatchesAsync(string matchHeaderId);
        Task<List<MatchHeader>> GetAllFriendlyMatchAsync();
        string ValidateFriendlyMatchDatas(FriendlyGameCreate datas);
        Task SetDeleteDateToNullAsync(string matchHeaderId);
    }
}
