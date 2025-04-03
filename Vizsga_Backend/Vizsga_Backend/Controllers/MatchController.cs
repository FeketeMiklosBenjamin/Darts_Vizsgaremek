using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Vizsga_Backend.Models.UserModels;
using Vizsga_Backend.Services;

namespace Vizsga_Backend.Controllers
{
    [Route("api/matches/")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchService;
        private readonly MatchHeaderService _matchHeaderService;

        public MatchController(MatchService matchService, MatchHeaderService matchHeaderService)
        {
            _matchService = matchService;
            _matchHeaderService = matchHeaderService;
        }

        [HttpGet("{matchId}")]
        [Authorize]
        public async Task<IActionResult> GetMatchById(string matchId)
        {
            try
            {
                var match = await _matchService.GetMatchWithPlayersByIdAsync(matchId);
                if (match == null)
                {
                    return NotFound(new { message = $"A mérkőzés az ID-vel ({matchId}) nem található." });
                }

                if (match.Status == "Finished")
                {
                    var resultFinished = new
                    {
                        match.Id,
                        match.Status,
                        startDate = match.StartDate,
                        match.RemainingPlayer,
                        match.RowNumber,
                        playerOne = new
                        {
                            match.PlayerOne!.Id,
                            match.PlayerOne.Username,
                            match.PlayerOne.ProfilePicture
                        },
                        playerTwo = new
                        {
                            match.PlayerTwo!.Id,
                            match.PlayerTwo.Username,
                            match.PlayerTwo.ProfilePicture
                        },
                        playerOneStat = new
                        {
                            match.PlayerOneStat!.Appeared,
                            match.PlayerOneStat.Won,
                            match.PlayerOneStat.SetsWon,
                            match.PlayerOneStat.LegsWon,
                            match.PlayerOneStat.Averages,
                            match.PlayerOneStat.Max180s,
                            match.PlayerOneStat.CheckoutPercentage,
                            match.PlayerOneStat.HighestCheckout,
                            match.PlayerOneStat.NineDarter,
                        },
                        playerTwoStat = new
                        {
                            match.PlayerTwoStat!.Appeared,
                            match.PlayerTwoStat.Won,
                            match.PlayerTwoStat.SetsWon,
                            match.PlayerTwoStat.LegsWon,
                            match.PlayerTwoStat.Averages,
                            match.PlayerTwoStat.Max180s,
                            match.PlayerTwoStat.CheckoutPercentage,
                            match.PlayerTwoStat.HighestCheckout,
                            match.PlayerTwoStat.NineDarter,
                        }
                    };
                    return Ok(resultFinished);
                }
                var resultPedding = new
                {
                    match.Id,
                    match.Status,
                    startDate = match.StartDate,
                    match.RemainingPlayer,
                    match.RowNumber,
                    playerOne = new
                    {
                        match.PlayerOne!.Id,
                        match.PlayerOne.Username,
                        match.PlayerOne.ProfilePicture
                    },
                    playerTwo = new
                    {
                        match.PlayerTwo!.Id,
                        match.PlayerTwo.Username,
                        match.PlayerTwo.ProfilePicture
                    }
                };

                return Ok(resultPedding);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

        [HttpPost("tournament/join/{matchId}")]
        [Authorize]
        public async Task<IActionResult> ValidateTournamentPassword(string matchId, [FromBody] string password)
        {
            try
            {
                var match = await _matchService.GetMatchByIdAsync(matchId);
                if (match == null)
                {
                    return NotFound(new { message = $"A mérkőzés az ID-vel ({matchId}) nem található." });
                }

                var matchHeader = await _matchHeaderService.GetByIdAsync(match.HeaderId);

                if (matchHeader == null)
                {
                    return NotFound(new { message = $"A mérkőzés fejléc az ID-vel ({match.HeaderId}) nem található." });
                }

                if (BCrypt.Net.BCrypt.Verify(password, matchHeader.JoinPassword))
                {
                    return Ok();
                }

                return BadRequest(new {message = "A megadott jelszó nem megfelelő!"});
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

        [HttpPost("friendy_match/join/{matchHeaderId}")]
        [Authorize]
        public async Task<IActionResult> ValidateFriendlyMatchPassword(string matchHeaderId, [FromBody] string password)
        {
            try
            {
                var matchHeader = await _matchHeaderService.GetByIdAsync(matchHeaderId);

                if (matchHeader == null)
                {
                    return NotFound(new { message = $"A mérkőzés fejléc az ID-vel ({matchHeaderId}) nem található." });
                }

                if (BCrypt.Net.BCrypt.Verify(password, matchHeader.JoinPassword))
                {
                    return Ok();
                }

                return BadRequest(new { message = "A megadott jelszó nem megfelelő!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }
    }
}
