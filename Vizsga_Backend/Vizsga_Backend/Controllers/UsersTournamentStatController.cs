using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using VizsgaBackend.Services;
using Vizsga_Backend.Models.UserStatsModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VizsgaBackend.Controllers
{
    [Route("api/users/tournamentstat/")]
    [ApiController]
    public class UsersTournamentStatController : ControllerBase
    {
        private readonly UsersTournamentStatService _service;

        public UsersTournamentStatController(UsersTournamentStatService service)
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
                    item.Averages,
                    item.Max180s,
                    item.CheckoutPercentage,
                    item.HighestCheckout,
                    item.NineDarter,
                    item.User!.Username,
                    profilePictureUrl = item.User.ProfilePicture,
                    registerDate = TimeZoneInfo.ConvertTimeFromUtc(item.User.RegisterDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd"),
                    lastLoginDate = TimeZoneInfo.ConvertTimeFromUtc(item.User.LastLoginDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm")
                }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }



        // GET api/<ProductController>/5
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
                    tournamentWithUser.Averages,
                    tournamentWithUser.Max180s,
                    tournamentWithUser.CheckoutPercentage,
                    tournamentWithUser.HighestCheckout,
                    tournamentWithUser.NineDarter,
                    tournamentWithUser.User!.Username,
                    profilePictureUrl = tournamentWithUser.User.ProfilePicture,
                    registerDate = TimeZoneInfo.ConvertTimeFromUtc(tournamentWithUser.User.RegisterDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd"),
                    lastLoginDate = TimeZoneInfo.ConvertTimeFromUtc(tournamentWithUser.User.LastLoginDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm")
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
                throw;
            }
        }


        // PUT api/<ProductController>/5
        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> Put(string userId, [FromBody] UsersTournamentStat updatedUserStat)
        {
            try
            {
                if (updatedUserStat == null)
                {
                    return BadRequest(new { message = "A kéréssel valami nincs rendben. Ellenőrizd az adatokat." });
                }

                var modifiedUser = await _service.GetTournamentByUserIdAsync(userId);
                if (modifiedUser == null)
                {
                    return NotFound(new { message = $"A felhasználó az ID-vel ({userId}) nem található." });
                }

                var filter = Builders<UsersTournamentStat>.Filter.Eq(u => u.UserId, userId); // Szűrés ID alapján
                var updateDefinitionBuilder = Builders<UsersTournamentStat>.Update;
                var updates = new List<UpdateDefinition<UsersTournamentStat>>();

                if (int.TryParse(updatedUserStat.Matches.ToString(), out int matches) && matches >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.Matches, matches));
                    modifiedUser.Matches = matches;
                }
                if (int.TryParse(updatedUserStat.MatchesWon.ToString(), out int matchesWon) && matchesWon >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.MatchesWon, matchesWon));
                    modifiedUser.MatchesWon = matchesWon;
                }
                if (int.TryParse(updatedUserStat.Sets.ToString(), out int sets) && sets >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.Sets, sets));
                    modifiedUser.Sets = sets;
                }
                if (int.TryParse(updatedUserStat.SetsWon.ToString(), out int setsWon) && setsWon >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.SetsWon, setsWon));
                    modifiedUser.SetsWon = setsWon;
                }
                if (int.TryParse(updatedUserStat.Legs.ToString(), out int legs) && legs >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.Legs, legs));
                    modifiedUser.Legs = legs;
                }
                if (int.TryParse(updatedUserStat.LegsWon.ToString(), out int legsWon) && legsWon >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.LegsWon, legsWon));
                    modifiedUser.LegsWon = legsWon;
                }
                if (int.TryParse(updatedUserStat.TournamentsWon.ToString(), out int tournamentWon) && tournamentWon >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.TournamentsWon, tournamentWon));
                    modifiedUser.TournamentsWon = tournamentWon;
                }
                if (int.TryParse(updatedUserStat.DartsPoints.ToString(), out int dartsPoints) && dartsPoints >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.DartsPoints, dartsPoints));
                    modifiedUser.DartsPoints = dartsPoints;
                }
                if (double.TryParse(updatedUserStat.Averages.ToString(), out double avarages) && avarages >= 0 && avarages <= 180)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.Averages, avarages));
                    modifiedUser.Averages = avarages;
                }
                if (int.TryParse(updatedUserStat.Max180s.ToString(), out int max180s) && max180s >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.Max180s, max180s));
                    modifiedUser.Max180s = max180s;
                }
                if (double.TryParse(updatedUserStat.CheckoutPercentage.ToString(), out double checkoutPercentage) && checkoutPercentage >= 0 && checkoutPercentage <= 100)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.CheckoutPercentage, checkoutPercentage));
                    modifiedUser.CheckoutPercentage = checkoutPercentage;
                }
                if (int.TryParse(updatedUserStat.HighestCheckout.ToString(), out int highestCheckout) && highestCheckout >= 2 && highestCheckout <= 170)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.HighestCheckout, highestCheckout));
                    modifiedUser.HighestCheckout = highestCheckout;
                }
                if (int.TryParse(updatedUserStat.NineDarter.ToString(), out int nineDarter) && nineDarter >= 0)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.NineDarter, nineDarter));
                    modifiedUser.NineDarter = nineDarter;
                }

                if (updates.Count == 0)
                {
                    return BadRequest(new { message = "Nincs frissíthető adat a kérelemben." });
                }
                else
                {
                    var updateDefinition = updateDefinitionBuilder.Combine(updates);
                    if (!IsValidStat(modifiedUser.Matches, modifiedUser.MatchesWon) || !IsValidStat(modifiedUser.Sets, modifiedUser.SetsWon) || !IsValidStat(modifiedUser.Legs, modifiedUser.LegsWon) || !IsValidStat(modifiedUser.Matches, modifiedUser.TournamentsWon))
                    {
                        return BadRequest(new { message = "A frissítés után logikai ellentmondás lépett fel az adatok között." });
                    }
                    else
                    {
                        var updateResult = await _service.UpdateOneAsync(filter, updateDefinition);
                        return Ok(new { message = $"A felhasználó az ID-vel ({userId}) sikeresen frissítve." });
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A módosítás során hiba történt." });
                throw;
            }
        }

        private bool IsValidStat(int? total, int? won)
        {
            if (total == null || won == null) return true;
            return won <= total;
        }
    }
}
