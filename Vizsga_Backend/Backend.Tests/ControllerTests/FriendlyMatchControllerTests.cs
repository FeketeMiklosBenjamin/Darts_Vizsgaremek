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
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Vizsga_Backend.Controllers;
using Vizsga_Backend.Interfaces;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.TournamentModels;
using Vizsga_Backend.Models.UserModels;
using Vizsga_Backend.Models.UserStatsModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Models;
using VizsgaBackend.Services;

namespace Backend.Tests.ControllerTests
{
    public class FriendlyMatchControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IMatchHeaderService> _mockMatchHeaderService;
        private readonly Mock<IUsersTournamentStatService> _mockUsersTournamentStatService;
        private readonly Mock<IOptions<MongoDBSettings>> _mockMongoDbSettings;
        private readonly IConfiguration _configuration;
        private readonly Mock<HttpContext> _mockHttpContext;

        private readonly FriendlyMatchController _controller;

        public FriendlyMatchControllerTests()
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

            _mockMatchHeaderService = new Mock<IMatchHeaderService>();
            _mockUsersTournamentStatService = new Mock<IUsersTournamentStatService>();
            _mockUserService = new Mock<IUserService>();

            _controller = new FriendlyMatchController(
                _mockMatchHeaderService.Object,
                _mockUsersTournamentStatService.Object,
                _mockUserService.Object
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
        public async Task GetAllFriendlyMatch_RegularUser_ReturnsMatches()
        {
            var userId = "user123";
            var email = "user@test.com";
            var role = "1";

            SetupAuthenticatedUser(userId, email, role);

            var friendlyMatches = new List<MatchHeader>
            {
                new MatchHeader
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
                new MatchHeader
                {
                    Name = "Amateur Tournament 2",
                    Level = "Amateur",
                    SetsCount = 4,
                    LegsCount = 3,
                    StartingPoint = 501,
                    BackroundImageUrl = "tournament_bg.jpg",
                    TournamentStartDate = DateTime.UtcNow.AddDays(10),
                    TournamentEndDate = DateTime.UtcNow.AddDays(20),
                    MatchDates = new List<DateTime>
                    {
                        DateTime.UtcNow.AddDays(10),
                        DateTime.UtcNow.AddDays(15),
                        DateTime.UtcNow.AddDays(20)
                    }
                },
            };

            _mockMatchHeaderService.Setup(x => x.GetAllFriendlyMatchAsync())
                .ReturnsAsync(friendlyMatches);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            var result = await _controller.GetAllFriendlyMatch();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JsonSerializer.Deserialize<List<JsonElement>>(JsonSerializer.Serialize(okResult.Value));

            Assert.NotNull(response);
            Assert.Equal(2, response.Count);

            var first = response[0];
            Assert.Equal("Amateur Tournament", first.GetProperty("Name").GetString());
            Assert.Equal("Amateur", first.GetProperty("playerLevel").GetString());
            Assert.Equal(3, first.GetProperty("SetsCount").GetInt32());
            Assert.Equal(5, first.GetProperty("LegsCount").GetInt32());
            Assert.Equal(501, first.GetProperty("StartingPoint").GetInt32());

            var second = response[1];
            Assert.Equal("Amateur Tournament 2", second.GetProperty("Name").GetString());
            Assert.Equal("Amateur", second.GetProperty("playerLevel").GetString());
            Assert.Equal(4, second.GetProperty("SetsCount").GetInt32());
            Assert.Equal(3, second.GetProperty("LegsCount").GetInt32());
            Assert.Equal(501, second.GetProperty("StartingPoint").GetInt32());
        }

        [Fact]
        public async Task GetAllFriendlyMatch_AdminUser_ReturnsUnauthorized()
        {
            var userId = "admin123";
            var email = "admin@test.com";
            var role = "2";

            SetupAuthenticatedUser(userId, email, role);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            var result = await _controller.GetAllFriendlyMatch();

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = JsonSerializer.Deserialize<dynamic>(JsonSerializer.Serialize(unauthorizedResult.Value));

            Assert.Equal("Csak felhasználók tudnak barátságos mérkőzéseket lekérdezni!",
                response.GetProperty("message").GetString());
        }

        [Fact]
        public async Task CreateFriendlyMatch_ValidData_ReturnsMatchId()
        {
            var userId = "user123";
            var email = "user@test.com";
            var role = "1";

            SetupAuthenticatedUser(userId, email, role);

            var matchData = new FriendlyGameCreate
            {
                LevelLocked = true,
                SetsCount = 3,
                LegsCount = 5,
                StartingPoint = 501,
                JoinPassword = "game123"
            };

            var userStat = new UsersTournamentStatWithUser
            {
                User = new User { Id = userId, Username = "dartPlayer" },
                Level = "Advanced"
            };

            _mockUsersTournamentStatService.Setup(x => x.GetTournamentWithUserByUserIdAsync(userId))
                .ReturnsAsync(userStat);
            _mockMatchHeaderService.Setup(x => x.ValidateFriendlyMatchDatas(matchData))
                .Returns("");
            _mockMatchHeaderService.Setup(x => x.CreateAsync(It.IsAny<MatchHeader>()))
                .Returns(Task.CompletedTask)
                .Callback<MatchHeader>(mh => mh.Id = "newMatch123");

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            var result = await _controller.CreateFriendlyMatch(matchData);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JsonSerializer.Deserialize<dynamic>(JsonSerializer.Serialize(okResult.Value));

            Assert.Equal("newMatch123", response.GetProperty("matchId").GetString());

            _mockMatchHeaderService.Verify(x => x.CreateAsync(It.Is<MatchHeader>(mh =>
                mh.Name == "dartPlayer" &&
                mh.Level == "Advanced" &&
                mh.SetsCount == 3 &&
                mh.LegsCount == 5 &&
                mh.StartingPoint == 501
            )), Times.Once);
        }

        [Fact]
        public async Task CreateFriendlyMatch_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var userId = "user123";
            var email = "user@test.com";
            var role = "1";

            SetupAuthenticatedUser(userId, email, role);

            var invalidData = new FriendlyGameCreate
            {
                LevelLocked = true,
                SetsCount = 0,
                LegsCount = 0,
                StartingPoint = 0
            };

            _mockUsersTournamentStatService.Setup(x => x.GetTournamentWithUserByUserIdAsync(userId))
                .ReturnsAsync(new UsersTournamentStatWithUser { User = new User(), Level = "Advanced" });
            _mockMatchHeaderService.Setup(x => x.ValidateFriendlyMatchDatas(invalidData))
                .Returns("Érvénytelen beállítások");

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            var result = await _controller.CreateFriendlyMatch(invalidData);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = JsonSerializer.Deserialize<dynamic>(JsonSerializer.Serialize(badRequestResult.Value));

            Assert.Equal("Érvénytelen beállítások", response.GetProperty("message").GetString());

            _mockMatchHeaderService.Verify(x => x.CreateAsync(It.IsAny<MatchHeader>()), Times.Never);
        }
    }
}