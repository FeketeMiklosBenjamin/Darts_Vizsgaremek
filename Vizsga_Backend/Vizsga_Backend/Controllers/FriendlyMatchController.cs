using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Services;

namespace Vizsga_Backend.Controllers
{
    [Route("api/friendly_matches")]
    [ApiController]
    public class FriendlyMatchController : ControllerBase
    {
        private readonly MatchHeaderService _matchHeaderService;
        private readonly UsersTournamentStatService _userTournamentStatService;
        private readonly UserService _userService;

        public FriendlyMatchController(MatchHeaderService matchHeaderService, UsersTournamentStatService userTournamentStatService, UserService userService)
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

                return Created();

            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A létrehozás során hiba történt." });
            }
        }

        [HttpPut("start/{matchHeaderId}")]
        [Authorize]
        public async Task<IActionResult> StartFriendlyMatch(string matchHeaderId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                var user = await _userService.GetByIdAsync(userId);

                var matchHeader = await _matchHeaderService.GetByIdAsync(matchHeaderId);

                if (matchHeader == null)
                {
                    return NotFound(new { message = $"A mérkőzés fejléc az ID-vel ({matchHeaderId}) nem található." });
                }

                if (user == null || user.Username != matchHeader.Name)
                {
                    return Unauthorized("Nincs jogod a verseny elkezdéséhez!");
                }

                await _matchHeaderService.SetDeleteDateToNullAsync(matchHeaderId);

                return Ok(new { message = "A mérkőzés sikeresen elindult" });

            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A modosítás során hiba történt." });
            }
        }

        [HttpDelete("{matchHeaderId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFriendlyMatch(string matchHeaderId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                var user = await _userService.GetByIdAsync(userId);

                var matchHeader = await _matchHeaderService.GetByIdAsync(matchHeaderId);

                if (matchHeader == null)
                {
                    return NotFound(new { message = $"A mérkőzés fejléc az ID-vel ({matchHeaderId}) nem található." });
                }

                if (user == null || user.Username != matchHeader.Name)
                {
                    return Unauthorized("Nincs jogod a verseny törléséhez!");
                }

                await _matchHeaderService.DeleteMatchHeaderAsync(matchHeaderId);

                return Ok(new {message = "A versenyt sikeresen töröltük"});
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A törlés során hiba történt." });
            }
        }
    }
}
