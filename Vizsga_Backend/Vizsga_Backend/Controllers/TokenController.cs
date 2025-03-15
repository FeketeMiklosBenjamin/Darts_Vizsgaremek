using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizsga_Backend.Models.UserModels;
using VizsgaBackend.Models;
using VizsgaBackend.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vizsga_Backend.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly UserService _userService;

        public TokenController(JwtService jwtService, UserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("refresh-token/{userId}")]
        public async Task<IActionResult> RefreshToken(string userId, [FromBody] RefreshTokenRequest request)
        {
            try
            {
                var user = await _userService.ValidateRefreshTokenAsync(request.RefreshToken);
                if (user == null)
                {
                    return Unauthorized(new { message = "Érvénytelen id vagy refresh token." });
                }
                if (user.Id != userId)
                {
                    return Unauthorized(new { message = "Érvénytelen id vagy refresh token." });
                }

                // Generáljunk egy új access token-t
                var accessTokenGen = _jwtService.GenerateToken(user.Id, user.EmailAddress, user.Role);
                await _userService.RefreshLastLoginDateAsync(user.Id);

                return Ok(new { accessToken = accessTokenGen });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A refresh token érvényesítése során hiba történt." });
            }
        }
    }
}
