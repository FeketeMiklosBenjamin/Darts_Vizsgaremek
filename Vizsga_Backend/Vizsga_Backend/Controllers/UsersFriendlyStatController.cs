using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;
using Vizsga_Backend.Models.SignalRModels;
using Vizsga_Backend.Models.UserStatsModels;
using VizsgaBackend.Services;

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
    }
}
