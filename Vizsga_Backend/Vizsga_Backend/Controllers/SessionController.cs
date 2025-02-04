using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizsga_Backend.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vizsga_Backend.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly SessionStore _sessionStore;

        public SessionController(SessionStore sessionStore)
        {
            _sessionStore = sessionStore;
        }

        [Authorize]
        [HttpPost("ping")]
        public async Task<IActionResult> Ping()
        {
            var sessionId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (sessionId == null)
                return Unauthorized();

            await _sessionStore.UpdateLastActivityAsync(sessionId);

            return Ok(new { message = "Session activity updated." });
        }
    }

}
