using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizsga_Backend.Models;
using Vizsga_Backend.Models.UserModels;
using Vizsga_Backend.Models.UserStatsModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vizsga_Backend.Controllers
{
    [Route("api/messages/")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _service;
        private readonly UserService _userService;

        public MessageController(MessageService service, UserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMessagesAsync()
        {
            try
            {

            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Az lekérés során hiba történt." });
                throw;
            }
        }

        [HttpPost("send/user")]
        [Authorize]
        public async Task<IActionResult> SendUserMessage([FromBody] Message message)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                if (userRole != "1")
                {
                    return Unauthorized(new { message = "Csak felhasználó küldhet üzenetet az adminnak!" });
                }

                if (message == null)
                {
                    return BadRequest(new { message = "Nem adott meg adatokat!" });
                }

                // Validáljuk a kötelező mezőket
                if (string.IsNullOrWhiteSpace(message.Title))
                {
                    return BadRequest(new { message = "Az üzenet címe nem lehet üres." });
                }

                if (string.IsNullOrWhiteSpace(message.Text))
                {
                    return BadRequest(new { message = "Az üzenet törzse nem lehet üres." });
                }

                message.FromId = userId;
                message.ToId = null;
                message.SendDate = DateTime.UtcNow;

                // Ha minden validálás sikeres, hozzáadjuk az új felhasználót az adatbázishoz

                await _service.CreateAsync(message);
                return Created();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Az elküldés során hiba történt." });
                throw;
            }
        }

        [HttpPost("send/admin")]
        [Authorize]
        public async Task<IActionResult> SendAdminMessage([FromBody] MessageSendAdmin message)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                var savedMessage = new Message();

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                if (userRole != "2")
                {
                    return Unauthorized(new { message = "Csak admin küldhet üzenetet a felhasználónak!" });
                }

                if (message == null)
                {
                    return BadRequest(new { message = "Nem adott meg adatokat!" });
                }

                // Validáljuk a kötelező mezőket
                if (string.IsNullOrWhiteSpace(message.Title))
                {
                    return BadRequest(new { message = "Az üzenet címe nem lehet üres." });
                }

                if (string.IsNullOrWhiteSpace(message.Text))
                {
                    return BadRequest(new { message = "Az üzenet törzse nem lehet üres." });
                }

                if (string.IsNullOrEmpty(message.EmailAddress))
                {
                    return BadRequest(new { message = "Az email cím megadása kötelező." });
                }

                // Ellenőrizzük, hogy létezik-e felhasználó az adott email címmel
                var user = await _userService.GetUserByEmailAsync(message.EmailAddress);
                if (user == null)
                {
                    return Unauthorized(new { message = "Ezzel az email címmel nem található felhasználó." });
                }

                savedMessage.Title = message.Title;
                savedMessage.Text = message.Text;
                savedMessage.FromId = null;
                savedMessage.ToId = user.Id;
                savedMessage.SendDate = DateTime.UtcNow;

                // Ha minden validálás sikeres, hozzáadjuk az új felhasználót az adatbázishoz

                await _service.CreateAsync(savedMessage);
                return Created();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Az elküldés során hiba történt." });
                throw;
            }
        }
    }
}
