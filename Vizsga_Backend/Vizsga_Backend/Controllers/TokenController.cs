using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizsga_Backend.Interfaces;
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
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public TokenController(IJwtService jwtService, IUserService userService)
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
