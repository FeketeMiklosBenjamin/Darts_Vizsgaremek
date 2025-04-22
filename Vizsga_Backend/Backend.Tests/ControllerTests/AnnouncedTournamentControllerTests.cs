using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Vizsga_Backend.Controllers;
using Vizsga_Backend.Interfaces;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.TournamentModels;
using Vizsga_Backend.Models.UserModels;
using Vizsga_Backend.Models.UserStatsModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Controllers;
using VizsgaBackend.Models;
using VizsgaBackend.Services;

namespace Backend.Tests.ControllerTests
{
    public class AnnouncedTournamentControllerTests
    {
        private readonly Mock<IAnnouncedTournamentService> _mockAnnouncedTournamentService;
        private readonly Mock<IMatchHeaderService> _mockMatchHeaderService;
        private readonly Mock<IUsersTournamentStatService> _mockUsersTournamentStatService;
        private readonly Mock<IMatchService> _mockMatchService;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Cloudinary _cloudinary;
        private readonly Mock<IOptions<MongoDBSettings>> _mockMongoDbSettings;
        private readonly IConfiguration _configuration;
        private readonly Mock<HttpContext> _mockHttpContext;

        private readonly AnnouncedTournamentController _controller;

        public AnnouncedTournamentControllerTests()
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

            _mockAnnouncedTournamentService = new Mock<IAnnouncedTournamentService>();
            _mockMatchService = new Mock<IMatchService>();
            _mockMatchHeaderService = new Mock<IMatchHeaderService>();
            _mockUsersTournamentStatService = new Mock<IUsersTournamentStatService>();
            _mockMessageService = new Mock<IMessageService>();

            _controller = new AnnouncedTournamentController(
                _mockAnnouncedTournamentService.Object,
                _mockMatchHeaderService.Object,
                _mockUsersTournamentStatService.Object,
                _mockMatchService.Object,
                _mockMessageService.Object,
                _cloudinary
            );
            _mockHttpContext = new Mock<HttpContext>();
        }

        private void SetupAuthenticatedUser(string userId, string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            var principal = new ClaimsPrincipal(identity);

            _mockHttpContext.Setup(x => x.User).Returns(principal);
        }

        [Fact]
        public async Task GetAllAnnouncedTournament_AdminUser_ReturnsFullDetails()
        {
            var userId = "507f1f77bcf86cd799439011";
            var role = "2";
            var email = "admin@example.com";

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = principal };
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var tournaments = new List<TournamentGetAll>
            {
                new TournamentGetAll
                {
                    Id = "62b8c1f6f8b2e75a6e8e8e8b",
                    HeaderId = "62b8c1f6f8b2e75a6e8e8e8c",
                    JoinStartDate = DateTime.UtcNow.AddDays(1),
                    JoinEndDate = DateTime.UtcNow.AddDays(7),
                    MaxPlayerJoin = 16,
                    MatchHeader = new MatchHeader
                    {
                        Name = "Amateur Tournament",
                        Level = "Amateur",
                        SetsCount = 3,
                        LegsCount = 5,
                        StartingPoint = 501,
                        BackroundImageUrl = "tournament_bg.jpg",
                        TournamentStartDate = DateTime.UtcNow.AddDays(8),
                        TournamentEndDate = DateTime.UtcNow.AddDays(10),
                        MatchDates = new List<DateTime>
                        {
                            DateTime.UtcNow.AddDays(8),
                            DateTime.UtcNow.AddDays(9),
                            DateTime.UtcNow.AddDays(10)
                        }
                    },
                    RegisteredPlayers = new List<User>
                    {
                        new User
                        {
                            Id = "661917f1e888ab6df5e2c456",
                            Username = "dartsKing91",
                            EmailAddress = "dartsking91@example.com",
                            ProfilePicture = "profilepics/king91.jpg"
                        },
                        new User
                        {
                            Id = "661918a5e888ab6df5e2c999",
                            Username = "dartQueen22",
                            EmailAddress = "dartqueen22@example.com",
                            ProfilePicture = "profilepics/queen22.jpg"
                        }
                    }
                }
            };

            _mockAnnouncedTournamentService.Setup(x => x.GetAnnouncedTournamentsWithPlayersAsync())
                .ReturnsAsync(tournaments);

            var result = await _controller.GetAllAnnouncedTournament();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JsonSerializer.Deserialize<List<JsonElement>>(JsonSerializer.Serialize(okResult.Value));

            Assert.NotNull(response);
            Assert.Single(response);

            var tournament = response[0];
            Assert.Equal("62b8c1f6f8b2e75a6e8e8e8b", tournament.GetProperty("Id").GetString());
            Assert.Equal("62b8c1f6f8b2e75a6e8e8e8c", tournament.GetProperty("HeaderId").GetString());
            Assert.Equal(16, tournament.GetProperty("MaxPlayerJoin").GetInt32());

            var matchHeader = tournament.GetProperty("matchHeader");
            Assert.Equal("Amateur Tournament", matchHeader.GetProperty("Name").GetString());
            Assert.Equal("Amateur", matchHeader.GetProperty("Level").GetString());
            Assert.Equal(3, matchHeader.GetProperty("SetsCount").GetInt32());
            Assert.Equal(5, matchHeader.GetProperty("LegsCount").GetInt32());

            var registeredPlayers = tournament.GetProperty("registeredPlayers");
            Assert.Equal(2, registeredPlayers.GetArrayLength());

            var firstPlayer = registeredPlayers[0];
            Assert.Equal("661917f1e888ab6df5e2c456", firstPlayer.GetProperty("Id").GetString());
            Assert.Equal("dartsKing91", firstPlayer.GetProperty("Username").GetString());
            Assert.Equal("dartsking91@example.com", firstPlayer.GetProperty("EmailAddress").GetString());

            var secondPlayer = registeredPlayers[1];
            Assert.Equal("661918a5e888ab6df5e2c999", secondPlayer.GetProperty("Id").GetString());
            Assert.Equal("dartQueen22", secondPlayer.GetProperty("Username").GetString());
            Assert.Equal("dartqueen22@example.com", secondPlayer.GetProperty("EmailAddress").GetString());
        }

        [Fact]
        public async Task CreateAnnouncedTournament_ValidDataAdminUser_ReturnsSuccess()
        {
            var userId = "admin123";
            var email = "admin@darts.com";
            var role = "2";

            SetupAuthenticatedUser(userId, email, role);

            var tournamentData = new TournamentCreate
            {
                Name = "Summer Dart Championship",
                SetsCount = 3,
                LegsCount = 5,
                StartingPoint = 501,
                Password = "securePass123",
                Level = "Bajnok",
                MaxPlayerJoin = 32,
                JoinStartDate = DateTime.UtcNow.AddDays(1),
                JoinEndDate = DateTime.UtcNow.AddDays(14),
                MatchDates = new List<DateTime>
                {
                    DateTime.UtcNow.AddDays(15),
                    DateTime.UtcNow.AddDays(16),
                    DateTime.UtcNow.AddDays(17)
                }
            };

            _mockAnnouncedTournamentService.Setup(x => x.ValidateCreateDatas(tournamentData))
                .Returns("");

            var createdMatchHeaderId = ObjectId.GenerateNewId().ToString();
            _mockMatchHeaderService.Setup(x => x.CreateAsync(It.IsAny<MatchHeader>()))
                .Returns(Task.CompletedTask)
                .Callback<MatchHeader>(mh => mh.Id = createdMatchHeaderId);

            _mockAnnouncedTournamentService.Setup(x => x.CreateTournamentAsync(It.IsAny<AnnouncedTournament>()))
                .Returns(Task.CompletedTask);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            var result = await _controller.CreateAnnouncedTournament(tournamentData);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(okResult.Value));

            Assert.Equal("Sikeres létrehozás!", response.GetProperty("message").GetString());
            Assert.NotNull(response.GetProperty("headerId").GetString());

            _mockMatchHeaderService.Verify(x => x.CreateAsync(It.Is<MatchHeader>(mh =>
                mh.Level == "Champion"
            )), Times.Once);
        }

        [Fact]
        public async Task CreateAnnouncedTournament_InvalidData_ReturnsBadRequest()
        {
            var userId = "admin123";
            var email = "admin@darts.com";
            var role = "2";

            SetupAuthenticatedUser(userId, email, role);

            var invalidData = new TournamentCreate
            {
                Name = "Invalid Tournament",
                SetsCount = 3,
                LegsCount = 5,
                StartingPoint = 501,
                Level = "Amateur",
                MaxPlayerJoin = 8,
                JoinStartDate = DateTime.UtcNow.AddDays(5),
                JoinEndDate = DateTime.UtcNow.AddDays(1),
                MatchDates = new List<DateTime>()
            };

            _mockAnnouncedTournamentService.Setup(x => x.ValidateCreateDatas(invalidData))
                .Returns("A jelszó megadása kötelező és a jelentkezési időszak vége nem lehet korábbi a kezdeténél");

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            var result = await _controller.CreateAnnouncedTournament(invalidData);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(badRequestResult.Value));

            Assert.Equal("A jelszó megadása kötelező és a jelentkezési időszak vége nem lehet korábbi a kezdeténél",
                response.GetProperty("message").GetString());

            _mockMatchHeaderService.Verify(x => x.CreateAsync(It.IsAny<MatchHeader>()), Times.Never);
        }
    }
}
