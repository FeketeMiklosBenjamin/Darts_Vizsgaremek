using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vizsga_Backend.Models.UserModels;
using Vizsga_Backend.Models.UserStatsModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Controllers;
using VizsgaBackend.Services;
using VizsgaBackend.Models;
using Microsoft.Extensions.Configuration;
using Xunit;
using Vizsga_Backend.Interfaces;
using System.Text.Json;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Security.Claims;
using Vizsga_Backend.Models.MatchModels;

namespace Backend.Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IUsersFriendlyStatService> _mockUserFriendlyStatService;
        private readonly Mock<IUsersTournamentStatService> _mockUserTournamentStatService;
        private readonly Mock<IMatchService> _mockMatchService;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly Cloudinary _cloudinary;
        private readonly Mock<IOptions<MongoDBSettings>> _mockMongoDbSettings;
        private readonly IConfiguration _configuration;
        private readonly Mock<HttpContext> _mockHttpContext;

        private readonly UserController _controller;

        private readonly List<UsersTournamentStatWithUser> testUsers = new List<UsersTournamentStatWithUser>
        {
            new UsersTournamentStatWithUser
            {
                Id = "661917f1e888ab6df5e2c123",
                UserId = "661917f1e888ab6df5e2c456",
                Matches = 10,
                MatchesWon = 6,
                Sets = 30,
                SetsWon = 18,
                Legs = 90,
                LegsWon = 60,
                TournamentsWon = 2,
                DartsPoints = 1750,
                Level = "Advanced",
                Averages = 71.3,
                Max180s = 12,
                CheckoutPercentage = 40.5,
                HighestCheckout = 161,
                NineDarter = 1,
                User = new User
                {
                    Id = "661917f1e888ab6df5e2c456",
                    Username = "dartsKing91",
                    Password = "hashedpassword123",
                    EmailAddress = "dartsking91@example.com",
                    ProfilePicture = "profilepics/king91.jpg",
                    Role = 0,
                    RegisterDate = DateTime.Parse("2024-11-12T15:30:00Z").ToUniversalTime(),
                    LastLoginDate = DateTime.Parse("2025-04-11T08:45:00Z").ToUniversalTime(),
                    StrictBan = false,
                    BannedUntil = null,
                    RefreshTokens = new List<string> { "token1", "token2" }
                }
            },
            new UsersTournamentStatWithUser
            {
                Id = "661918a5e888ab6df5e2c789",
                UserId = "661918a5e888ab6df5e2c999",
                Matches = 20,
                MatchesWon = 14,
                Sets = 50,
                SetsWon = 35,
                Legs = 150,
                LegsWon = 100,
                TournamentsWon = 5,
                DartsPoints = 2500,
                Level = "Advanced",
                Averages = 82.7,
                Max180s = 20,
                CheckoutPercentage = 48.9,
                HighestCheckout = 170,
                NineDarter = 2,
                User = new User
                {
                    Id = "661918a5e888ab6df5e2c999",
                    Username = "dartQueen22",
                    Password = "securehash456",
                    EmailAddress = "dartqueen22@example.com",
                    ProfilePicture = "profilepics/queen22.jpg",
                    Role = 0,
                    RegisterDate = DateTime.Parse("2024-10-05T10:00:00Z").ToUniversalTime(),
                    LastLoginDate = DateTime.Parse("2025-04-11T17:25:00Z").ToUniversalTime(),
                    StrictBan = false,
                    BannedUntil = DateTime.UtcNow.AddDays(1),
                    RefreshTokens = new List<string> { "tokenA", "tokenB" }
                }
            }
        };

        public UserControllerTests()
        {
            _mockMongoDbSettings = new Mock<IOptions<MongoDBSettings>>();
            _mockMongoDbSettings.Setup(m => m.Value).Returns(new MongoDBSettings
            {
                ConnectionURI = "mongodb://localhost:27017",
                DatabaseName = "TestDb",
                UsersCollectionName = "users",
                UsersTournamentStatsCollectionName = "users_tournament_stats",
                UsersFriendlyStatsCollectionName = "users_friendly_stats",
                MessagesCollectionName = "messages",
                MatchHeadersCollectionName = "match_headers",
                AnnouncedTournamentsCollectionName = "announced_tournaments",
                PlayersTournamentsCollectionName = "players-tournaments",
                MatchesCollectionName = "matches",
                Players_Match_StatsCollectionName = "players_match_stats"
            });

            var configValues = new Dictionary<string, string>
        {
            {"Jwt:SecretKey", "DartsVizsgaRemekBackendMcFozve2030"},
            {"Jwt:Issuer", "test-issuer"},
            {"Jwt:Audience", "test-audience"}
        };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            _cloudinary = new Cloudinary(new Account("cloud_name", "api_key", "api_secret"));

            _mockUserService = new Mock<IUserService>();
            _mockUserFriendlyStatService = new Mock<IUsersFriendlyStatService>();
            _mockUserTournamentStatService = new Mock<IUsersTournamentStatService>();
            _mockMatchService = new Mock<IMatchService>();

            // Mockoljuk a JwtService-t
            _mockJwtService = new Mock<IJwtService>();

            // Generáljunk egy egyszerű token mockot
            _mockJwtService.Setup(service => service.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns("mock_access_token");

            _mockJwtService.Setup(service => service.GenerateRefreshToken())
                .Returns("mock_refresh_token");

            _controller = new UserController(
                _mockUserService.Object,
                _mockUserFriendlyStatService.Object,
                _mockUserTournamentStatService.Object,
                _mockMatchService.Object,
                _mockJwtService.Object,  // Itt injektáljuk a mockolt JwtService-t
                _cloudinary
            );
            _mockHttpContext = new Mock<HttpContext>();
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithUsers_WhenSuccessful()
        {
            _mockUserTournamentStatService
                .Setup(x => x.GetTournamentsWithUsersAsync())
                .ReturnsAsync(testUsers);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var json = JsonSerializer.Serialize(okResult.Value);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var users = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json, options);

            Assert.NotNull(users);
            Assert.Equal(2, users!.Count);

            var firstUser = users[0];
            Assert.Equal("661917f1e888ab6df5e2c456", firstUser["id"].ToString());
            Assert.Equal("dartsKing91", firstUser["username"].ToString());
            Assert.Equal("dartsking91@example.com", firstUser["emailAddress"].ToString());
            Assert.Equal("profilepics/king91.jpg", firstUser["profilePictureUrl"].ToString());
            Assert.Equal("Advanced", firstUser["level"].ToString());
            Assert.Equal(JsonValueKind.Number, ((JsonElement)firstUser["dartsPoints"]).ValueKind);
            Assert.Equal(1750, ((JsonElement)firstUser["dartsPoints"]).GetInt32());
            Assert.Equal(null, firstUser["bannedUntil"]);

            var secondUser = users[1];
            Assert.Equal("661918a5e888ab6df5e2c999", secondUser["id"].ToString());
            Assert.Equal("dartQueen22", secondUser["username"].ToString());
            Assert.Equal("dartqueen22@example.com", secondUser["emailAddress"].ToString());
            Assert.Equal("profilepics/queen22.jpg", secondUser["profilePictureUrl"].ToString());
            Assert.Equal("Advanced", secondUser["level"].ToString());
            Assert.Equal(2500, ((JsonElement)secondUser["dartsPoints"]).GetInt32());
            Assert.NotEqual(JsonValueKind.Null, ((JsonElement)secondUser["bannedUntil"]).ValueKind);
        }

        [Fact]
        public async Task GetLeaderboard_ReturnsOkWithUsers_WhenSuccessful()
        {
            _mockUserTournamentStatService
                .Setup(x => x.GetTournamentsWithUsersNotStrictBannedAsync())
                .ReturnsAsync(testUsers);

            var result = await _controller.GetLeaderboard();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var json = JsonSerializer.Serialize(okResult.Value);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var users = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json, options);

            Assert.NotNull(users);
            Assert.Equal(2, users.Count);

            var firstUser = users[0];
            Assert.Equal("661917f1e888ab6df5e2c456", firstUser["id"].ToString());
            Assert.Equal("dartsKing91", firstUser["username"].ToString());
            Assert.Equal("dartsking91@example.com", firstUser["emailAddress"].ToString());
            Assert.Equal("profilepics/king91.jpg", firstUser["profilePictureUrl"].ToString());
            Assert.Equal("Advanced", firstUser["level"].ToString());
            Assert.Equal(JsonValueKind.Number, ((JsonElement)firstUser["dartsPoints"]).ValueKind);
            Assert.Equal(1750, ((JsonElement)firstUser["dartsPoints"]).GetInt32());

            var secondUser = users[1];
            Assert.Equal("661918a5e888ab6df5e2c999", secondUser["id"].ToString());
            Assert.Equal("dartQueen22", secondUser["username"].ToString());
            Assert.Equal("dartqueen22@example.com", secondUser["emailAddress"].ToString());
            Assert.Equal("profilepics/queen22.jpg", secondUser["profilePictureUrl"].ToString());
            Assert.Equal("Advanced", secondUser["level"].ToString());
            Assert.Equal(2500, ((JsonElement)secondUser["dartsPoints"]).GetInt32());
        }

        [Fact]
        public async Task GetAll_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            _mockUserTournamentStatService
                .Setup(service => service.GetTournamentsWithUsersAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.GetAll();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(objectResult.Value));

            Assert.NotNull(response);
            Assert.True(response.ContainsKey("message"));
            Assert.Equal("A lekérés során hiba történt.", response["message"]);
        }

        [Fact]
        public async Task GetLeaderboard_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            _mockUserTournamentStatService
                .Setup(service => service.GetTournamentsWithUsersNotStrictBannedAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.GetLeaderboard();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(
                JsonSerializer.Serialize(objectResult.Value));

            Assert.NotNull(response);
            Assert.True(response.ContainsKey("message"));
            Assert.Equal("A lekérés során hiba történt.", response["message"]);
        }

        [Fact]
        public async Task Register_ReturnsUnauthorized_WhenUserAlreadyLoggedIn()
        {
            // Arrange
            var testUser = new User
            {
                Username = "testuser",
                EmailAddress = "test@example.com",
                Password = "password123"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Közvetlenül hozzáadjuk a headert
            _controller.HttpContext.Request.Headers.Add("Authorization", "Bearer some_token");

            // Act
            var result = await _controller.Register(testUser);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(unauthorizedResult.Value));
            Assert.NotNull(response);
            Assert.Equal("Már be van jelentkezve, nem regisztrálhat új fiókot.", response["message"]);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUsernameIsEmpty()
        {
            // Arrange
            var testUser = new User
            {
                Username = "",
                EmailAddress = "test@example.com",
                Password = "password123"
            };

            // Mockoljuk a HTTP kérést (üres headers)
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _mockUserService.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(true);
            _mockUserService.Setup(x => x.IsEmailTakenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            _mockUserService.Setup(x => x.CreateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            _mockUserFriendlyStatService.Setup(x => x.CreateAsync(It.IsAny<UsersFriendlyStat>())).Returns(Task.CompletedTask);
            _mockUserTournamentStatService.Setup(x => x.CreateAsync(It.IsAny<UsersTournamentStat>())).Returns(Task.CompletedTask);

            _mockJwtService.Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns("mock_token");
            _mockJwtService.Setup(x => x.GenerateRefreshToken()).Returns("mock_refresh_token");
            _mockUserService.Setup(x => x.SaveRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(testUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(badRequestResult.Value));
            Assert.NotNull(response);
            Assert.Equal("A felhasználónév nem lehet üres.", response["message"]);

            _mockUserService.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenEmailIsEmpty()
        {
            var testUser = new User
            {
                Username = "testuser",
                EmailAddress = "",
                Password = "password123"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _mockUserService.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(true);
            _mockUserService.Setup(x => x.IsEmailTakenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var result = await _controller.Register(testUser);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(badRequestResult.Value));
            Assert.NotNull(response);
            Assert.Equal("Az email cím nem lehet üres.", response["message"]);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenEmailIsInvalid()
        {
            var testUser = new User
            {
                Username = "testuser",
                EmailAddress = "invalidemail",
                Password = "password123"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _mockUserService.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(false);
            _mockUserService.Setup(x => x.IsEmailTakenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var result = await _controller.Register(testUser);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(badRequestResult.Value));
            Assert.NotNull(response);
            Assert.Equal("Az email cím nem megfelelő.", response["message"]);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenPasswordIsTooShort()
        {
            var testUser = new User
            {
                Username = "testuser",
                EmailAddress = "test@example.com",
                Password = "short"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _mockUserService.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(true);
            _mockUserService.Setup(x => x.IsEmailTakenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var result = await _controller.Register(testUser);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(badRequestResult.Value));
            Assert.NotNull(response);
            Assert.Equal("A jelszónak legalább 8 karakter hosszúnak kell lennie.", response["message"]);
        }

        [Fact]
        public async Task Register_ReturnsConflict_WhenEmailAlreadyTaken()
        {
            var testUser = new User
            {
                Username = "testuser",
                EmailAddress = "taken@example.com",
                Password = "password123"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _mockUserService.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(true);
            _mockUserService.Setup(x => x.IsEmailTakenAsync(testUser.EmailAddress, null)).ReturnsAsync(true);

            var result = await _controller.Register(testUser);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(conflictResult.Value));
            Assert.NotNull(response);
            Assert.Equal("Ez az email cím már regisztrálva van.", response["message"]);
        }

        [Fact]
        public async Task Register_ReturnsOkWithUserData_WhenRegistrationIsSuccessful()
        {
            var testUser = new User
            {
                Username = "newuser",
                EmailAddress = "new@example.com",
                Password = "password123"
            };

            var createdUser = new User
            {
                Id = "12345",
                Username = testUser.Username,
                EmailAddress = testUser.EmailAddress,
                ProfilePicture = "https://res.cloudinary.com/dvikunqov/image/upload/v1740128607/darts_profile_pictures/fvlownxvkn4etrkvfutl.jpg",
                Role = 1
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _mockUserService.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(true);
            _mockUserService.Setup(x => x.IsEmailTakenAsync(testUser.EmailAddress, null)).ReturnsAsync(false);
            _mockUserService.Setup(x => x.CreateAsync(It.IsAny<User>())).Returns(Task.CompletedTask)
                .Callback<User>(u => u.Id = createdUser.Id);
            _mockUserService.Setup(x => x.SaveRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _mockUserFriendlyStatService.Setup(x => x.CreateAsync(It.IsAny<UsersFriendlyStat>())).Returns(Task.CompletedTask);
            _mockUserTournamentStatService.Setup(x => x.CreateAsync(It.IsAny<UsersTournamentStat>())).Returns(Task.CompletedTask);

            _mockJwtService.Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns("mock_access_token");
            _mockJwtService.Setup(x => x.GenerateRefreshToken()).Returns("mock_refresh_token");

            var result = await _controller.Register(testUser);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(okResult.Value));
            Assert.NotNull(response);
            Assert.Equal("Sikeres regisztráció.", response["message"].ToString());
            Assert.Equal("12345", response["id"].ToString());
            Assert.Equal("newuser", response["username"].ToString());
            Assert.Equal("new@example.com", response["emailAddress"].ToString());
            Assert.Equal("https://res.cloudinary.com/dvikunqov/image/upload/v1740128607/darts_profile_pictures/fvlownxvkn4etrkvfutl.jpg", response["profilePictureUrl"].ToString());
            Assert.Equal(0, ((JsonElement)response["dartsPoints"]).GetInt32());
            Assert.Equal("Amateur", response["level"].ToString());
            Assert.Equal(1, ((JsonElement)response["role"]).GetInt32());
            Assert.Equal("mock_access_token", response["accessToken"].ToString());
            Assert.Equal("mock_refresh_token", response["refreshToken"].ToString());
        }

        [Fact]
        public async Task Register_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            var testUser = new User
            {
                Username = "testuser",
                EmailAddress = "test@example.com",
                Password = "password123"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _mockUserService.Setup(x => x.IsValidEmail(It.IsAny<string>())).Returns(true);
            _mockUserService.Setup(x => x.IsEmailTakenAsync(testUser.EmailAddress, null))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.Register(testUser);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(objectResult.Value));
            Assert.NotNull(response);
            Assert.Equal("A létrehozás során hiba történt.", response["message"]);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithTokens()
        {
            // Arrange
            var loginRequest = new Login { EmailAddress = "test@test.com", Password = "password" };
            var user = new User
            {
                EmailAddress = "test@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password"),
                Role = 1
            };

            _mockUserService.Setup(x => x.GetUserByEmailAsync(loginRequest.EmailAddress))
                .ReturnsAsync(user);

            _mockJwtService.Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns("access_token");
            _mockJwtService.Setup(x => x.GenerateRefreshToken())
                .Returns("refresh_token");

            _mockUserTournamentStatService.Setup(x => x.GetTournamentByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new UsersTournamentStat
                {
                    Id = "some-id",
                    UserId = "user-id",
                    Matches = 20,
                    MatchesWon = 15,
                    Sets = 40,
                    SetsWon = 30,
                    Legs = 120,
                    LegsWon = 90,
                    TournamentsWon = 5,
                    DartsPoints = 2000,
                    Level = "Advanced",
                    Averages = 75.0,
                    Max180s = 10,
                    CheckoutPercentage = 45.6,
                    HighestCheckout = 160,
                    NineDarter = 2
                });

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Használjunk egy névtelen típust a deszerializáláshoz
            var response = JsonSerializer.Deserialize<dynamic>(JsonSerializer.Serialize(okResult.Value));

            // A dinamikus mezők elérése indexeléssel
            string message = response.GetProperty("message").GetString();
            string accessToken = response.GetProperty("accessToken").GetString();
            string refreshToken = response.GetProperty("refreshToken").GetString();

            Assert.Equal("Sikeres bejelentkezés.", message);
            Assert.Equal("access_token", accessToken);
            Assert.Equal("refresh_token", refreshToken);
        }

        [Fact]
        public async Task Login_InvalidEmail_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new Login { EmailAddress = "wrong@test.com", Password = "password" };
            _mockUserService.Setup(x => x.GetUserByEmailAsync(loginRequest.EmailAddress))
                .ReturnsAsync((User)null);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = JsonSerializer.Deserialize<dynamic>(JsonSerializer.Serialize(unauthorizedResult.Value));
            Assert.Equal("Hibás email cím vagy jelszó.", response.GetProperty("message").GetString());
        }

        [Fact]
        public async Task Put_ValidUsernameChange_ReturnsOk()
        {
            // Arrange
            var userId = "123";
            var modifyUser = new ModifyUser
            {
                Username = "newUsername",
                OldPassword = "correctPassword"
            };

            var existingUser = new User
            {
                Id = userId,
                Password = BCrypt.Net.BCrypt.HashPassword("correctPassword")
            };

            SetupAuthenticatedUser(userId);
            _mockUserService.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Itt lazítunk a feltételen, mert a konkrét implementáció más típust használ
            var mockUpdateResult = new Mock<UpdateResult>();
            mockUpdateResult.Setup(x => x.IsAcknowledged).Returns(true);
            mockUpdateResult.Setup(x => x.ModifiedCount).Returns(1);

            // Mockoljuk az UpdateOneAsync-t is
            _mockUserService.Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<User>>(), It.IsAny<UpdateDefinition<User>>()))
                .ReturnsAsync(mockUpdateResult.Object);

            // Act
            var result = await _controller.Put(modifyUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Sikeres módosítás!", GetResponseMessage(okResult.Value));
        }

        [Fact]
        public async Task Put_InvalidCurrentPassword_ReturnsUnauthorized()
        {
            // Arrange
            var userId = "123";
            var modifyUser = new ModifyUser
            {
                Username = "newUsername",
                OldPassword = "wrongPassword"
            };

            var existingUser = new User
            {
                Id = userId,
                Password = BCrypt.Net.BCrypt.HashPassword("correctPassword")
            };

            SetupAuthenticatedUser(userId);
            _mockUserService.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act
            var result = await _controller.Put(modifyUser);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Hibás mostani jelszó.", GetResponseMessage(unauthorizedResult.Value));
        }

        [Fact]
        public async Task GetUserTournamentMatches_WithUpcomingMatches_ReturnsMatches()
        {
            var userId = "507f1f77bcf86cd799439011";
            var opponentId = "507f191e810c19729de860ea";
            var matches = new List<MatchWithPlayers> {
                new MatchWithPlayers
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Header = new MatchHeader
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = "Tournament 1",
                        Level = "Amateur",
                        SetsCount = 5,
                        LegsCount = 10,
                        StartingPoint = 501,
                        IsDrawed = false
                    },
                    StartDate = DateTime.UtcNow.AddDays(1),
                    PlayerOne = new User
                    {
                        Id = userId,
                        Username = "currentUser",
                        Password = "hashed_password",
                        EmailAddress = "currentuser@test.com",
                        ProfilePicture = "/images/currentuser.jpg",
                        Role = 1,
                        RegisterDate = DateTime.UtcNow.AddMonths(-1),
                        RefreshTokens = new List<string> { "token1", "token2" },
                        LastLoginDate = DateTime.UtcNow,
                        StrictBan = false,
                        BannedUntil = null
                    },
                    PlayerTwo = new User
                    {
                        Id = opponentId,
                        Username = "opponent1",
                        Password = "hashed_password",
                        EmailAddress = "opponent1@test.com",
                        ProfilePicture = "/images/opponent1.jpg",
                        Role = 1,
                        RegisterDate = DateTime.UtcNow.AddMonths(-2),
                        RefreshTokens = new List<string> { "token3", "token4" },
                        LastLoginDate = DateTime.UtcNow.AddHours(-2),
                        StrictBan = false,
                        BannedUntil = null
                    },
                    Status = "Started"
                },
                new MatchWithPlayers
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Header = new MatchHeader
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = "Tournament 2",
                        Level = "Amateur",
                        SetsCount = 3,
                        LegsCount = 9,
                        StartingPoint = 301,
                        IsDrawed = false
                    },
                    StartDate = DateTime.UtcNow.AddDays(2),
                    PlayerOne = new User
                    {
                        Id = opponentId,
                        Username = "opponent2",
                        Password = "hashed_password",
                        EmailAddress = "opponent2@test.com",
                        ProfilePicture = "/images/opponent2.jpg",
                        Role = 1,
                        RegisterDate = DateTime.UtcNow.AddMonths(-3),
                        RefreshTokens = new List<string> { "token5", "token6" },
                        LastLoginDate = DateTime.UtcNow.AddHours(-3),
                        StrictBan = false,
                        BannedUntil = null
                    },
                    PlayerTwo = new User
                    {
                        Id = userId,
                        Username = "currentUser",
                        Password = "hashed_password",
                        EmailAddress = "currentuser@test.com",
                        ProfilePicture = "/images/currentuser.jpg",
                        Role = 1,
                        RegisterDate = DateTime.UtcNow.AddMonths(-1),
                        RefreshTokens = new List<string> { "token1", "token2" },
                        LastLoginDate = DateTime.UtcNow,
                        StrictBan = false,
                        BannedUntil = null
                    },
                    Status = "Started"
                }
            };
            SetupAuthenticatedUser(userId);
            _mockMatchService.Setup(x => x.GetUserUpcomingMatchesAsync(userId, null))
                .ReturnsAsync(matches);

            // Act
            // Előkészületek
            var result = await _controller.GetUserTournamentMatches();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JsonSerializer.Deserialize<List<dynamic>>(JsonSerializer.Serialize(okResult.Value));

            Assert.Equal(2, response.Count);

            // Ellenőrizzük az első mérkőzés adatait
            Assert.Equal("Tournament 1", response[0].GetProperty("Name").GetString());
            Assert.Equal("Amateur", response[0].GetProperty("Level").GetString());
            Assert.Equal("opponent1", response[0].GetProperty("opponentName").GetString());

            // Ellenőrizzük a második mérkőzés adatait
            Assert.Equal("Tournament 2", response[1].GetProperty("Name").GetString());
            Assert.Equal("Amateur", response[1].GetProperty("Level").GetString());
            Assert.Equal("opponent2", response[1].GetProperty("opponentName").GetString());

        }

        [Fact]
        public async Task GetUserTournamentMatches_NoMatches_ReturnsNotFound()
        {
            // Arrange
            var userId = "507f1f77bcf86cd799439011";
            SetupAuthenticatedUser(userId);
            _mockMatchService.Setup(x => x.GetUserUpcomingMatchesAsync(userId, null))
                .ReturnsAsync((List<MatchWithPlayers>)null);

            // Act
            var result = await _controller.GetUserTournamentMatches();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Nincsen közelgő verseny mérkőzése", GetResponseMessage(notFoundResult.Value));
        }

        private void SetupAuthenticatedUser(string userId)
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        private string GetResponseMessage(object response)
        {
            var json = JsonSerializer.Serialize(response);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return dict["message"];
        }
    }
}
