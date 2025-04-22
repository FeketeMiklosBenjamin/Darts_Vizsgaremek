using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Vizsga_Backend.Models;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Services;

namespace Vizsga_Backend.Controllers
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly IMatchHeaderService _matchHeaderService;
        private readonly IMatchService _matchService;

        public TournamentController(IMatchHeaderService matchHeaderService, IMatchService matchService)
        {
            _matchHeaderService = matchHeaderService;
            _matchService = matchService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTournamentHeader()
        {
            try
            {
                var tournaments = await _matchHeaderService.GetAllDrawedTournamentAsync();

                var result = tournaments.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Level,
                    x.BackroundImageUrl,
                    tournamentStartDate = x.TournamentStartDate,
                    tournamentEndDate = x.TournamentEndDate,
                });
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

        [HttpGet("{matchHeaderId}")]
        [Authorize]
        public async Task<IActionResult> GetTournamentWithMatches(string matchHeaderId)
        {
            try
            {
                var tournament = await _matchHeaderService.GetTournamentWithMatchesAsync(matchHeaderId);

                var peddingMatches = tournament.Matches.Where(x => x.Status == "Pedding").ToList();
                var finishedNotSetMatches = tournament.Matches.Where(match => (match.PlayerOneStat == null || match.PlayerTwoStat == null) && match.Status == "Finished").ToList();

                if (peddingMatches.Any())
                {
                    peddingMatches.ForEach(async match =>
                    {
                        if (match.StartDate!.Value.AddMinutes(20) < DateTime.UtcNow)
                        {
                            await _matchService.SetAllPlayerStatNotAppearedAsync(match.Id, null);
                        }
                    });
                }

                if (finishedNotSetMatches.Any())
                {
                    finishedNotSetMatches.ForEach(async match =>
                    {
                        await _matchService.SetAllPlayerStatNotAppearedAsync(match.Id, (match.PlayerOneStat == null ? match.PlayerOne!.Id : match.PlayerTwo!.Id));
                    });
                }

                if (tournament == null)
                {
                    return NotFound(new { message = $"A verseny az ID-vel ({matchHeaderId}) nem található." });
                }

                if (!tournament.IsDrawed)
                {
                    return BadRequest(new { message = $"A(z) {matchHeaderId} ID-vel verseny még nincs kisorsolva." });
                }

                var result = new
                {
                    tournament.Id,
                    tournament.Name,
                    tournament.Level,
                    tournament.SetsCount,
                    tournament.LegsCount,
                    tournament.StartingPoint,
                    tournament.BackroundImageUrl,
                    tournamentStartDate = tournament.TournamentStartDate,
                    tournamentEndDate = tournament.TournamentEndDate,
                    matches = tournament.Matches.Select(match => new
                    {
                        match.Id,
                        match.Status,
                        startDate = match.StartDate,
                        match.RemainingPlayer,
                        match.RowNumber,
                        playerOne = new
                        {
                            match.PlayerOne!.Id,
                            match.PlayerOne.Username
                        },
                        playerTwo = new
                        {
                            match.PlayerTwo!.Id,
                            match.PlayerTwo.Username
                        },
                        match.PlayerOneStat?.Won,
                        playerOneResult = match.PlayerOneStat?.SetsWon == 0 ? match.PlayerOneStat?.LegsWon : match.PlayerOneStat?.SetsWon,
                        playerTwoResult = match.PlayerTwoStat?.SetsWon == 0 ? match.PlayerTwoStat?.LegsWon : match.PlayerTwoStat?.SetsWon,
                    })

                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

    }
}
