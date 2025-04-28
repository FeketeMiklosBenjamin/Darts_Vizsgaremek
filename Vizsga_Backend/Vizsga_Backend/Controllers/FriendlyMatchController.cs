using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizsga_Backend.Interfaces;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Services;

namespace Vizsga_Backend.Controllers
{
    [Route("api/friendly_matches")]
    [ApiController]
    public class FriendlyMatchController : ControllerBase
    {
        private readonly IMatchHeaderService _matchHeaderService;
        private readonly IUsersTournamentStatService _userTournamentStatService;
        private readonly IUserService _userService;

        public FriendlyMatchController(IMatchHeaderService matchHeaderService, IUsersTournamentStatService userTournamentStatService, IUserService userService)
        {
            _matchHeaderService = matchHeaderService;
            _userTournamentStatService = userTournamentStatService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllFriendlyMatch()
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole == "2")
                {
                    return Unauthorized(new { message = "Csak felhasználók tudnak barátságos mérkőzéseket lekérdezni!" });
                }

                var friendlyMatches = await _matchHeaderService.GetAllFriendlyMatchAsync();

                var result = friendlyMatches.Select(x => new
                {
                    x.Id,
                    x.Name,
                    playerLevel = x.Level.Split(" ")[0],
                    levelLocked = x.Level.Split(" ").Count() == 1,
                    x.SetsCount,
                    x.LegsCount,
                    x.StartingPoint,
                    x.JoinPassword
                });
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFriendlyMatch([FromBody] FriendlyGameCreate matchdatas)
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                if (userRole == "2")
                {
                    return Unauthorized(new { message = "Csak felhasználók tudnak barátságos mérkőzéseket létrehozni!" });
                }

                var userWithTournamentStat = await _userTournamentStatService.GetTournamentWithUserByUserIdAsync(userId);

                string errorMessage = _matchHeaderService.ValidateFriendlyMatchDatas(matchdatas);

                if (errorMessage != "")
                {
                    return BadRequest(new { message = errorMessage });
                }

                MatchHeader newFriendyMatch = new MatchHeader
                {
                    Name = userWithTournamentStat!.User!.Username,
                    Level = matchdatas.LevelLocked ? $"{userWithTournamentStat.Level!}" : $"{userWithTournamentStat.Level} All",
                    DeleteDate = DateTime.UtcNow.AddHours(1),
                    SetsCount = matchdatas.SetsCount,
                    LegsCount = matchdatas.LegsCount,
                    StartingPoint = matchdatas.StartingPoint,
                    JoinPassword = matchdatas.JoinPassword == null ? null : BCrypt.Net.BCrypt.HashPassword(matchdatas.JoinPassword),
                    BackroundImageUrl = null,
                    TournamentStartDate = null,
                    TournamentEndDate = null,
                    IsDrawed = false
                };

                await _matchHeaderService.CreateAsync(newFriendyMatch);

                return Ok(new
                {
                    matchId = newFriendyMatch.Id
                });

            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A létrehozás során hiba történt." });
            }
        }
    }
}
