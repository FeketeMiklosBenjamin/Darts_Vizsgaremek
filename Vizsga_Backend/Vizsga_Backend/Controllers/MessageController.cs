using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizsga_Backend.Interfaces;
using Vizsga_Backend.Models.MessageModels;
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
        private readonly IMessageService _service;
        private readonly IUserService _userService;

        public MessageController(IMessageService service, IUserService userService)
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                if (userRole == "1")
                {
                    var messages = await _service.GetUserMessagesAsync(userId);

                    var result = messages.Select(m => new
                    {
                        m.Id,
                        m.Title,
                        m.Text,
                        sendDate = m.SendDate,
                    });
                    return Ok(result);
                }

                if (userRole == "2")
                {
                    var messages = await _service.GetAdminMessagesAsync();

                    var result = messages.Select(m => new
                    {
                        m.Id,
                        m.Title,
                        m.Text,
                        sendDate = m.SendDate,
                        username = m.User!.Username,
                        emailAddress = m.User.EmailAddress
                    });
                    return Ok(result);
                }
                return Unauthorized(new {message = "A lekérés során hiba történt."});
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
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

        [HttpDelete("{messageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(string messageId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                var message = new Message();

                if (userRole == "1")
                {
                    message = await _service.GetMessageAsync(userId, messageId);
                }
                else if (userRole == "2")
                {
                    message = await _service.GetMessageAsync(null, messageId);
                }

                if (message == null)
                {
                    return NotFound(new { message = $"Az üzenet az ID-vel ({messageId}) nem található vagy nincs jogod az üzenet törléséhez." });
                }
                await _service.DeleteAsync(messageId);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Az törlés során hiba történt." });
                throw;
            }
        }
    }
}
