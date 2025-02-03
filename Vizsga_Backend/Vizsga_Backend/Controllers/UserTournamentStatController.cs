using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VizsgaBackend.Models;
using VizsgaBackend.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VizsgaBackend.Controllers
{
    [Route("api/users/tournamentstat")]
    [ApiController]
    public class UserTournamentStatController : ControllerBase
    {
        private readonly UserTournamentStatService _service;

        public UserTournamentStatController(UserTournamentStatService service)
        {
            _service = service;
        }

        // GET api/<ProductController>/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            try
            {
                var item = await _service.GetByUserIdAsync(userId);
                if (item == null)
                    return NotFound(new { message = $"A felhasználó barátságos statisztikái az ID-vel ({userId}) nem található." });
                return Ok(item);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
                throw;
            }
        }


        // PUT api/<ProductController>/5
        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(string userId, [FromBody] UserTournamentStat updatedUserStat)
        {
            try
            {
                if (updatedUserStat == null)
                {
                    return BadRequest(new { message = "A kéréssel valami nincs rendben. Ellenőrizd az adatokat." });
                }

                var modifiedUser = await _service.GetByUserIdAsync(userId);
                if (modifiedUser == null)
                {
                    return NotFound(new { message = $"A felhasználó az ID-vel ({userId}) nem található." });
                }

                var filter = Builders<UserTournamentStat>.Filter.Eq(u => u.UserId, userId); // Szűrés ID alapján
                var updateDefinitionBuilder = Builders<UserTournamentStat>.Update;
                var updates = new List<UpdateDefinition<UserTournamentStat>>();

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
