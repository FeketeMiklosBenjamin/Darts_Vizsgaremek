using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vizsga_Backend.Services;

namespace Vizsga_Backend.Controllers
{
    [Route("api/matches/")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchService;

        public MatchController(MatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("{matchId}")]
        [Authorize]
        public async Task<IActionResult> GetMatchById(string matchId)
        {
            try
            {
                var match = await _matchService.GetMatchByIdAsync(matchId);
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
                        startDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)match.StartDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
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
                    startDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)match.StartDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
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
    }
}
