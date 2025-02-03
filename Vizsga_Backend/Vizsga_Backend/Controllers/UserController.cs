using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using VizsgaBackend.Models;
using VizsgaBackend.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VizsgaBackend.Controllers
{
    [Route("api/users/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly UserFriendlyStatService _userFriendlyStatService;
        private readonly UserTournamentStatService _userTournamentStatService;
        private readonly JwtService _jwtService;

        public UserController(UserService service, UserFriendlyStatService userFriendlyStatService, UserTournamentStatService userTournamentStatService, JwtService jwtService)
        {
            _service = service;
            _userFriendlyStatService = userFriendlyStatService;
            _userTournamentStatService = userTournamentStatService;
            _jwtService = jwtService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _service.GetAsync();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
                throw;
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user == null)
                    return NotFound(new {message = $"A felhasználó az ID-vel ({id}) nem található." });
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
                throw;
            }
        }

        // POST api/<ProductController>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User registerUser)
        {
            try
            {
                if (registerUser == null)
                {
                    return BadRequest(new {message = "Nem adott meg adatokat a regisztráláshoz" });
                }

                // Validáljuk a kötelező mezőket
                if (string.IsNullOrEmpty(registerUser.Username))
                {
                    return BadRequest(new {message = "Az felhasználónév nem lehet üres." });
                }

                if (string.IsNullOrEmpty(registerUser.EmailAddress))
                {
                    return BadRequest(new {message = "Az email cím nem lehet üres." });
                }

                if (!_service.IsValidEmail(registerUser.EmailAddress))
                {
                    return BadRequest(new {message = "Az email cím nem megfelelő." });
                }

                if (string.IsNullOrEmpty(registerUser.Password) || registerUser.Password.Length < 8)
                {
                    return BadRequest(new {message = "A jelszónak legalább 8 karakter hosszúnak kell lennie." });
                }

                if (registerUser.Role != 1 && registerUser.Role != 2)
                {
                    return BadRequest(new {message = "Az role csak 1 és 2 lehet." });
                }

                // Ellenőrizzük, hogy az email cím már létezik-e az adatbázisban
                var existingUser = await _service.IsEmailTakenAsync(registerUser.EmailAddress, null);
                if (existingUser)
                {
                    return Conflict(new {message = "Ez az email cím már regisztrálva van." });
                }

                // Jelszó titkosítása
                registerUser.Password = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

                // Ha minden validálás sikeres, hozzáadjuk az új felhasználót az adatbázishoz
                registerUser.RegisterDate = DateTime.UtcNow;
                await _service.CreateAsync(registerUser);
                var userFriendlyStat = new UserFriendlyStat
                {
                    UserId = registerUser.Id,
                    Matches = 0,
                    MatchesWon = 0,
                    Sets = 0,
                    SetsWon = 0,
                    Legs = 0,
                    LegsWon = 0,
                    Averages = 0,
                    Max180s = 0,
                    CheckoutPercentage = 0,
                    HighestCheckout = 0,
                    NineDarter = 0
                };
                var userTournamentStat = new UserTournamentStat
                {
                    UserId = registerUser.Id,
                    Matches = 0,
                    MatchesWon = 0,
                    Sets = 0,
                    SetsWon = 0,
                    Legs = 0,
                    LegsWon = 0,
                    TournamentsWon = 0,
                    DartsPoints = 0,
                    Averages = 0,
                    Max180s = 0,
                    CheckoutPercentage = 0,
                    HighestCheckout = 0,
                    NineDarter = 0
                };

                await _userFriendlyStatService.CreateAsync(userFriendlyStat);
                await _userTournamentStatService.CreateAsync(userTournamentStat);

                // Válasz küldése a sikeres regisztrációról, az új entitással
                return CreatedAtAction(nameof(GetById), new { id = registerUser.Id }, registerUser);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A létrehozás során hiba történt." });
                throw;
            }
        }

        // Bejelentkezés
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            try
            {
                if (loginRequest == null)
                {
                    return BadRequest(new { message = "A bejelentkezéshez szükséges adatok nem érvényesek." });
                }

                if (string.IsNullOrEmpty(loginRequest.EmailAddress) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return BadRequest(new { message = "Az email cím és jelszó megadása kötelező." });
                }

                // Ellenőrizzük, hogy létezik-e felhasználó az adott email címmel
                var user = await _service.GetUserByEmailAsync(loginRequest.EmailAddress);
                if (user == null)
                {
                    return Unauthorized(new { message = "Hibás email cím vagy jelszó." });
                }

                // Ellenőrizzük a jelszót
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
                if (!isPasswordValid)
                {
                    return Unauthorized(new { message = "Hibás email cím vagy jelszó." });
                }

                var tokenString = _jwtService.GenerateToken(user.Id, user.EmailAddress, user.Role);

                // Sikeres bejelentkezés esetén válasz
                return Ok(new { message = "Sikeres bejelentkezés.", userId = user.Id, token = tokenString });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A bejelentkezés során hiba történt." });
            }
        }


        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] User updatedUser)
        {
            try
            {
                if (updatedUser == null)
                {
                    return BadRequest(new { message = "A kéréssel valami nincs rendben. Ellenőrizd az adatokat." });
                }

                var modifyUser = await _service.GetByIdAsync(id);
                if (modifyUser == null)
                {
                    return NotFound(new { message = $"A felhasználó az ID-vel ({id}) nem található." });
                }
                var filter = Builders<User>.Filter.Eq(u => u.Id, id);
                var updateDefinitionBuilder = Builders<User>.Update;
                var updates = new List<UpdateDefinition<User>>();

                if (!string.IsNullOrEmpty(updatedUser.Username))
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.Username, updatedUser.Username));
                }

                if (!string.IsNullOrEmpty(updatedUser.Password))
                {
                    if (updatedUser.Password.Length > 8)
                    {
                        updates.Add(updateDefinitionBuilder.Set(u => u.Password, updatedUser.Password));
                    }
                    else
                    {
                        return BadRequest(new { message = "A jelszónak legalább 8 karakter hosszúnak kell lennie." });
                    }
                }

                if (updatedUser.Role == 1 || updatedUser.Role == 2)
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.Role, updatedUser.Role));
                }

                if (await _service.IsEmailTakenAsync(updatedUser.EmailAddress, id))
                {
                    return BadRequest(new { message = "Az email cím már foglalt." });
                }

                if (!string.IsNullOrEmpty(updatedUser.EmailAddress))
                {
                    if (_service.IsValidEmail(updatedUser.EmailAddress))
                    {
                        updates.Add(updateDefinitionBuilder.Set(u => u.EmailAddress, updatedUser.EmailAddress));
                    }
                    else
                    {
                        return BadRequest(new { message = "Az email cím nem megfelelő." });
                    }
                }

                if (updates.Count == 0)
                {
                    return BadRequest(new { message = "Nincs frissíthető adat a kérelemben." });
                }

                var updateDefinition = updateDefinitionBuilder.Combine(updates);

                await _service.UpdateOneAsync(filter, updateDefinition);

                return Ok(new { message = $"A felhasználó az ID-vel ({id}) sikeresen frissítve." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A módosítás során hiba történt." });
                throw;
            }
        }


        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole != "2")
                {
                    return Unauthorized(new { message = "Nincs jogosultságod a törléshez." });
                }

                var existing = await _service.GetByIdAsync(id);

                if (existing == null)
                {
                    return NotFound(new { message = $"A felhasználó az ID-val ({id}) nem található." });
                }

                await _service.DeleteAsync(id);
                await _userFriendlyStatService.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A törlés során hiba történt." });
            }
        }

    }
}
