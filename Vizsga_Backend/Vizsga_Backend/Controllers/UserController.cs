using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.UserModels;
using Vizsga_Backend.Models.UserStatsModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VizsgaBackend.Controllers
{
    [Route("api/users/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly UsersFriendlyStatService _userFriendlyStatService;
        private readonly UsersTournamentStatService _userTournamentStatService;
        private readonly MatchService _matchService;
        private readonly JwtService _jwtService;
        private readonly Cloudinary _cloudinary;

        public UserController(UserService service, UsersFriendlyStatService userFriendlyStatService, UsersTournamentStatService userTournamentStatService, MatchService matchService, JwtService jwtService, Cloudinary cloudinary)
        {
            _service = service;
            _userFriendlyStatService = userFriendlyStatService;
            _userTournamentStatService = userTournamentStatService;
            _matchService = matchService;
            _jwtService = jwtService;
            _cloudinary = cloudinary;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userTournamentStatService.GetTournamentsWithUsersAsync();

                var result = users.Select(user => new
                {
                    id = user.User!.Id,
                    username = user.User.Username,
                    emailAddress = user.User.EmailAddress,
                    profilePictureUrl = user.User.ProfilePicture,
                    level = user.Level,
                    dartsPoints = user.DartsPoints
                }).ToList();

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

        [HttpGet("leaderboard")]
        [Authorize]
        public async Task<IActionResult> GetLeaderboard()
        {
            try
            {
                var users = await _userTournamentStatService.GetTournamentsWithUsersNotStrictBannedAsync();

                var result = users.Select(user => new
                {
                    id = user.User!.Id,
                    username = user.User.Username,
                    emailAddress = user.User.EmailAddress,
                    profilePictureUrl = user.User.ProfilePicture,
                    level = user.Level,
                    dartsPoints = user.DartsPoints
                }).ToList();

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

        // Nem kell
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user == null)
                    return NotFound(new {message = $"A felhasználó az ID-vel ({id}) nem található." });

                var result = new
                {
                    user.Id,
                    user.Username,
                    user.EmailAddress,
                    profilePictureUrl = user.ProfilePicture,
                    registerDate = TimeZoneInfo.ConvertTimeFromUtc(user.RegisterDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd"),
                    lastLoginDate = TimeZoneInfo.ConvertTimeFromUtc(user.LastLoginDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm")
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
                throw;
            }
        }

        [HttpPost("register/admin")]
        [Authorize]
        public async Task<IActionResult> RegisterAdmin([FromBody] User registerUser)
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole != "2")
                {
                    return Unauthorized(new { message = "Nincs jogosultságod admin regisztrációhoz." });
                }

                if (registerUser == null)
                {
                    return BadRequest(new { message = "Nem adott meg adatokat a regisztráláshoz" });
                }

                // Validáljuk a kötelező mezőket
                if (string.IsNullOrWhiteSpace(registerUser.Username))
                {
                    return BadRequest(new { message = "A felhasználónév nem lehet üres." });
                }

                if (string.IsNullOrWhiteSpace(registerUser.EmailAddress))
                {
                    return BadRequest(new { message = "Az email cím nem lehet üres." });
                }

                if (!_service.IsValidEmail(registerUser.EmailAddress))
                {
                    return BadRequest(new { message = "Az email cím nem megfelelő." });
                }

                if (string.IsNullOrWhiteSpace(registerUser.Password) || registerUser.Password.Length < 8)
                {
                    return BadRequest(new { message = "A jelszónak legalább 8 karakter hosszúnak kell lennie." });
                }

                var existingUser = await _service.IsEmailTakenAsync(registerUser.EmailAddress, null);
                if (existingUser)
                {
                    return Conflict(new { message = "Ez az email cím már regisztrálva van." });
                }

                // Jelszó titkosítása
                registerUser.Password = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

                // Ha minden validálás sikeres, hozzáadjuk az új admint az adatbázishoz
                registerUser.Role = 2;
                registerUser.RegisterDate = DateTime.UtcNow;
                registerUser.ProfilePicture = "https://res.cloudinary.com/dvikunqov/image/upload/v1740128607/darts_profile_pictures/fvlownxvkn4etrkvfutl.jpg";
                registerUser.StrictBan = false;
                registerUser.BannedUntil = null;

                await _service.CreateAsync(registerUser);

                // Válasz küldése a sikeres regisztrációról, az új entitással
                return CreatedAtAction(nameof(GetById), new { id = registerUser.Id }, new
                {
                    message = $"Sikeres regisztráció! Felhasználónév: {registerUser.Username}, E-mail cím: {registerUser.EmailAddress}"
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A létrehozás során hiba történt." });
                throw;
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User registerUser)
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized(new { message = "Már be van jelentkezve, nem regisztrálhat új fiókot." });
                }

                if (registerUser == null)
                {
                    return BadRequest(new {message = "Nem adott meg adatokat a regisztráláshoz" });
                }

                // Validáljuk a kötelező mezőket
                if (string.IsNullOrWhiteSpace(registerUser.Username))
                {
                    return BadRequest(new {message = "A felhasználónév nem lehet üres." });
                }

                if (string.IsNullOrWhiteSpace(registerUser.EmailAddress))
                {
                    return BadRequest(new {message = "Az email cím nem lehet üres." });
                }

                if (!_service.IsValidEmail(registerUser.EmailAddress))
                {
                    return BadRequest(new {message = "Az email cím nem megfelelő." });
                }

                if (string.IsNullOrWhiteSpace(registerUser.Password) || registerUser.Password.Length < 8)
                {
                    return BadRequest(new {message = "A jelszónak legalább 8 karakter hosszúnak kell lennie." });
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
                registerUser.Role = 1;
                registerUser.RegisterDate = DateTime.UtcNow;
                registerUser.ProfilePicture = "https://res.cloudinary.com/dvikunqov/image/upload/v1740128607/darts_profile_pictures/fvlownxvkn4etrkvfutl.jpg";
                registerUser.StrictBan = false;
                registerUser.BannedUntil = null;

                await _service.CreateAsync(registerUser);

                var userFriendlyStat = new UsersFriendlyStat
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
                var userTournamentStat = new UsersTournamentStat
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
                    Level = "Amateur",
                    Averages = 0,
                    Max180s = 0,
                    CheckoutPercentage = 0,
                    HighestCheckout = 0,
                    NineDarter = 0
                };

                await _userFriendlyStatService.CreateAsync(userFriendlyStat);
                await _userTournamentStatService.CreateAsync(userTournamentStat);

                var accessTokenGen = _jwtService.GenerateToken(registerUser.Id, registerUser.EmailAddress, registerUser.Role);

                var refreshTokenGen = _jwtService.GenerateRefreshToken();

                await _service.SaveRefreshTokenAsync(registerUser.Id, refreshTokenGen);

                // Válasz küldése a sikeres regisztrációról, az új entitással
                return CreatedAtAction(nameof(GetById), new { id = registerUser.Id }, new {
                    message = "Sikeres regisztráció.",
                    id = registerUser.Id,
                    username = registerUser.Username,
                    emailAddress = registerUser.EmailAddress,
                    profilePictureUrl = registerUser.ProfilePicture,
                    dartsPoints = userTournamentStat.DartsPoints,
                    level = userTournamentStat.Level,
                    role = registerUser.Role,
                    accessToken = accessTokenGen,
                    refreshToken = refreshTokenGen
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A létrehozás során hiba történt." });
                throw;
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized(new { message = "Már be van jelentkezve." });
                }

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

                //Ellenőrizzük, hogy nincs-e bannolva
                if (user.BannedUntil != null)
                {
                    if (user.BannedUntil > DateTime.UtcNow)
                    {
                        string userBannedUntilDate = TimeZoneInfo.ConvertTimeFromUtc(user.BannedUntil.Value, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm");
                        return Unauthorized(new { message = $"A profilja tiltva van van eddig: {userBannedUntilDate}" });
                    }
                    else
                    {
                        await _service.SetUserBanAsync(user.Id, false, null);
                    }
                }


                var userTournamentStats = await _userTournamentStatService.GetTournamentByUserIdAsync(user.Id);

                if (userTournamentStats == null && user.Role != 2)
                {
                    return NotFound(new {message = "A felhasználó statisztikája nem található!"});
                }

                var accessTokenGen = _jwtService.GenerateToken(user.Id, user.EmailAddress, user.Role);

                var refreshTokenGen = _jwtService.GenerateRefreshToken();

                await _service.SaveRefreshTokenAsync(user.Id, refreshTokenGen);

                // Válasz a sikeres bejelentkezés után
                return Ok(new
                {
                    message = "Sikeres bejelentkezés.",
                    id = user.Id,
                    username = user.Username,
                    emailAddress = user.EmailAddress,
                    profilePictureUrl = user.ProfilePicture,
                    dartsPoints = userTournamentStats != null ? userTournamentStats.DartsPoints : null,
                    level = userTournamentStats != null ? userTournamentStats.Level : null,
                    role = user.Role,
                    accessToken = accessTokenGen,
                    refreshToken = refreshTokenGen
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A bejelentkezés során hiba történt." });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userById = await _service.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (userById == null)
                {
                    return Unauthorized(new { message = "A felhasználó nincs bejelentkezve." });
                }
                var userByToken = await _service.ValidateRefreshTokenAsync(request.RefreshToken);
                if (userByToken == null || (userById.Id != userByToken.Id))
                {
                    return BadRequest(new { message = "Rossz refresh token." });
                }

                // Töröljük a refresh tokent az adatbázisból
                await _service.DeleteRefreshTokenAsync(userById.Id, request.RefreshToken);

                // Visszajelzés a sikeres kijelentkezésről
                return Ok(new { message = "Sikeres kijelentkezés." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A kijelentkezés során hiba történt." });
            }
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] ModifyUser modifyUser)
        {
            try
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                if (modifyUser == null)
                {
                    return BadRequest(new { message = "A kéréssel valami nincs rendben. Ellenőrizd az adatokat." });
                }

                var updatedUser = await _service.GetByIdAsync(userId);
                if (updatedUser == null)
                {
                    return NotFound(new { message = $"A felhasználó az ID-vel ({userId}) nem található." });
                }

                // Ellenőrizzük a jelszót
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(modifyUser.Password, updatedUser.Password);
                if (!isPasswordValid)
                {
                    return Unauthorized(new { message = "Hibás mostani jelszó." });
                }

                var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
                var updateDefinitionBuilder = Builders<User>.Update;
                var updates = new List<UpdateDefinition<User>>();

                if (!string.IsNullOrWhiteSpace(modifyUser.Username))
                {
                    updates.Add(updateDefinitionBuilder.Set(u => u.Username, modifyUser.Username));
                }

                if (!string.IsNullOrWhiteSpace(modifyUser.NewPassword))
                {
                    if (modifyUser.NewPassword.Length > 8)
                    {
                        updates.Add(updateDefinitionBuilder.Set(u => u.Password, BCrypt.Net.BCrypt.HashPassword(modifyUser.NewPassword)));
                    }
                    else
                    {
                        return BadRequest(new { message = "A jelszónak legalább 8 karakter hosszúnak kell lennie." });
                    }
                }

                if (!string.IsNullOrWhiteSpace(modifyUser.EmailAddress))
                {
                    if (_service.IsValidEmail(modifyUser.EmailAddress))
                    {
                        if (!await _service.IsEmailTakenAsync(modifyUser.EmailAddress, userId))
                        {
                            updates.Add(updateDefinitionBuilder.Set(u => u.EmailAddress, modifyUser.EmailAddress));
                        }
                        else
                        {
                            return BadRequest(new { message = "Az email cím már foglalt." });
                        }
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

                return Ok(new { message = $"A felhasználó az ID-vel ({userId}) sikeresen frissítve." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A módosítás során hiba történt." });
                throw;
            }
        }


        [HttpPut("ban/{userId}")]
        [Authorize]
        public async Task<IActionResult> BanUser(string userId, [FromBody] BanRequest request)
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole != "2")
                {
                    return Unauthorized(new { message = "Nincs jogosultságod a kitiltáshoz." });
                }
                if (request.BanDuration < 0)
                {
                    return BadRequest(new { message = "A kitiltási időnek pozitív számnak kell lennie!" });
                }

                var user = await _service.GetByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = $"A felhasználó az ID-vel ({userId}) nem található." });
                }

                if (request.BanDuration == 0)
                {
                    await _service.SetUserBanAsync(user.Id, false, null);
                    return Ok(new { message = $"A(z) {user.EmailAddress} e-mail címmel rendelkező felhasználó tiltása fel lett oldva." });
                }

                var bannedUntilDate = DateTime.UtcNow.AddDays(request.BanDuration);

                await _service.SetUserBanAsync(user.Id, request.BanDuration >= 30, bannedUntilDate);

                string userBannedUntilDateString = TimeZoneInfo.ConvertTimeFromUtc(bannedUntilDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm");

                return Ok(new { message = $"A(z) {user.EmailAddress} e-mail címmel rendelkező felhasználó tiltva lett eddig: {userBannedUntilDateString}" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A kitiltás során hiba történt." });
                throw;
            }
        }


        [HttpPost("picture/upload")]
        [Authorize]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "Nem adott meg fájlt!" });

                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "darts_profile_pictures"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                await _service.DeleteProfilePictureAsync(userId!);
                await _service.SaveProfilePictureAsync(userId!, uploadResult.SecureUrl.ToString());

                return Ok(new { profilePictureUrl = uploadResult.SecureUrl.ToString() });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A feltöltés során hiba történt." });
            }
        }

        [HttpGet("{userId}/lastmatches")]
        [Authorize]
        public async Task<IActionResult> GetUserLastMatches(string userId)
        {
            try
            {
                var user = await _service.GetByIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = $"A felhasználó az ID-vel ({userId}) nem található." });

                int previousMatchesCount = 2;

                List<MatchToCard> AllMatchesResult = new List<MatchToCard>();

                MatchToCard? matchUpcommingResult = null;

                var matchUpcommings = await _matchService.GetUserUpcomingMatchesAsync(userId, 1);

                var matchUpcomming = matchUpcommings == null ? null : matchUpcommings[0];

                if (matchUpcomming == null)
                {
                    previousMatchesCount = 3;
                }
                else
                {
                    matchUpcommingResult = new MatchToCard
                    (
                        matchUpcomming.Id,
                        matchUpcomming.Header!.Id,
                        matchUpcomming.Header.Name,
                        matchUpcomming.Status,
                        TimeZoneInfo.ConvertTimeFromUtc((DateTime)matchUpcomming.StartDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        matchUpcomming.PlayerOne!.Username,
                        matchUpcomming.PlayerTwo!.Username,
                        null,
                        null,
                        null
                    );

                    AllMatchesResult.Add(matchUpcommingResult);
                }

                var previousMatches = await _matchService.GetUserLastMatchesAsync(userId, previousMatchesCount);

                foreach ( var match in previousMatches )
                {
                    bool userWon = false;

                    if (match.PlayerOne!.Id == userId && match.PlayerOneStat!.Won)
                    {
                        userWon = true;
                    }
                    else if (match.PlayerTwo!.Id == userId && match.PlayerTwoStat!.Won)
                    {
                        userWon = true;
                    }

                    MatchToCard matchResult = new MatchToCard
                    (
                        match.Id,
                        match.Header!.Id,
                        match.Header.Name,
                        match.Status,
                        TimeZoneInfo.ConvertTimeFromUtc((DateTime)match.StartDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        match.PlayerOne!.Username,
                        match.PlayerTwo!.Username,
                        match.PlayerOneStat!.SetsWon ?? match.PlayerOneStat.LegsWon,
                        match.PlayerTwoStat!.SetsWon ?? match.PlayerTwoStat.LegsWon,
                        userWon
                    );

                    AllMatchesResult.Add(matchResult);
                }

                return Ok(new {matches = AllMatchesResult} );
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

        [HttpGet("tournament_matches")]
        [Authorize]
        public async Task<IActionResult> GetUserTournamentMatches()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                var matchesUpcomming = await _matchService.GetUserUpcomingMatchesAsync(userId, null);

                if (matchesUpcomming == null)
                {
                    return NotFound(new { message = "Nincsen közelgő verseny mérkőzése" });
                }
                else
                {
                    var result = matchesUpcomming.Select(match => new
                    {
                        match.Id,
                        match.Header!.Name,
                        match.Header.Level,
                        startDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)match.StartDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        opponentName = match.PlayerOne!.Id != userId ? match.PlayerOne.Username : match.PlayerTwo!.Username,
                    });
                    return Ok(result);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

    }
}
