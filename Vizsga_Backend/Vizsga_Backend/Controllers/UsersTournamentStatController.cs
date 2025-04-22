using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using VizsgaBackend.Services;
using Vizsga_Backend.Models.UserStatsModels;
using Microsoft.AspNetCore.Authorization;
using Vizsga_Backend.Models.SignalRModels;

namespace VizsgaBackend.Controllers
{
    [Route("api/users/tournamentstat/")]
    [ApiController]
    public class UsersTournamentStatController : ControllerBase
    {
        private readonly IUsersTournamentStatService _service;

        public UsersTournamentStatController(IUsersTournamentStatService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTournamentStat()
        {
            try
            {
                var tournamentsWithUsers = await _service.GetTournamentsWithUsersAsync();
                var result = tournamentsWithUsers.Select(item => new
                {
                    item.Id,
                    item.UserId,
                    item.Matches,
                    item.MatchesWon,
                    item.Sets,
                    item.SetsWon,
                    item.Legs,
                    item.LegsWon,
                    item.TournamentsWon,
                    item.DartsPoints,
                    item.Level,
                    item.Averages,
                    item.Max180s,
                    item.CheckoutPercentage,
                    item.HighestCheckout,
                    item.NineDarter,
                    item.User!.Username,
                    profilePictureUrl = item.User.ProfilePicture,
                    registerDate = item.User.RegisterDate,
                    lastLoginDate = item.User.LastLoginDate
                }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }


        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetTournamentByUserId(string userId)
        {
            try
            {
                var tournamentWithUser = await _service.GetTournamentWithUserByUserIdAsync(userId);
                if (tournamentWithUser == null)
                    return NotFound(new { message = $"A felhasználó verseny statisztikái az ID-vel ({userId}) nem található." });

                return Ok(new
                {
                    tournamentWithUser.Id,
                    tournamentWithUser.UserId,
                    tournamentWithUser.Matches,
                    tournamentWithUser.MatchesWon,
                    tournamentWithUser.Sets,
                    tournamentWithUser.SetsWon,
                    tournamentWithUser.Legs,
                    tournamentWithUser.LegsWon,
                    tournamentWithUser.TournamentsWon,
                    tournamentWithUser.DartsPoints,
                    tournamentWithUser.Level,
                    tournamentWithUser.Averages,
                    tournamentWithUser.Max180s,
                    tournamentWithUser.CheckoutPercentage,
                    tournamentWithUser.HighestCheckout,
                    tournamentWithUser.NineDarter,
                    tournamentWithUser.User!.Username,
                    profilePictureUrl = tournamentWithUser.User.ProfilePicture,
                    registerDate = tournamentWithUser.User.RegisterDate,
                    lastLoginDate = tournamentWithUser.User.LastLoginDate
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
                throw;
            }
        }
    }
}
