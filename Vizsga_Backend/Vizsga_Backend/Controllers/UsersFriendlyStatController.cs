using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;
using Vizsga_Backend.Models.SignalRModels;
using Vizsga_Backend.Models.UserStatsModels;
using VizsgaBackend.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VizsgaBackend.Controllers
{
    [Route("api/users/friendlystat/")]
    [ApiController]
    public class UsersFriendlyStatController : ControllerBase
    {
        private readonly IUsersFriendlyStatService _service;

        public UsersFriendlyStatController(IUsersFriendlyStatService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByUserId()
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized(new { message = "Még nem vagy bejelentkezve!" });
                }
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

        //[HttpPut("test/{userId}")]
        //public async Task<IActionResult> TestSaveStat(string userId, [FromBody] EndMatchModel stat)
        //{
        //    try
        //    {
        //        if (stat == null)
        //            return BadRequest(new { message = "A statisztikai adat nem lehet null." });

        //        var result = await _service.SavePlayerStat(userId, stat);

        //        return Ok(new
        //        {
        //            message = "Statisztika frissítve teszt módban.",
        //            ModifiedCount = result?.ModifiedCount ?? 0
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "A teszt stat frissítése során hiba történt.", error = ex.Message });
        //    }
        //}
    }
}
