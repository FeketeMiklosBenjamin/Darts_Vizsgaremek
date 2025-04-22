using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;
using System.Text.Json;
using Vizsga_Backend.Controllers;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.UserModels;
using Vizsga_Backend.Models.UserStatsModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Models;

namespace Backend.Tests.ControllerTests
{
    public class MatchControllerTests
    {
        private readonly Mock<IMatchService> _mockMatchService;
        private readonly Mock<IMatchHeaderService> _mockMatchHeaderService;
        private readonly Mock<IOptions<MongoDBSettings>> _mockMongoDbSettings;
        private readonly IConfiguration _configuration;
        private readonly Mock<HttpContext> _mockHttpContext;

        private readonly MatchController _controller;

        public MatchControllerTests()
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
            _mockMatchService = new Mock<IMatchService>();

            _controller = new MatchController(
                _mockMatchService.Object,
                _mockMatchHeaderService.Object
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
        public async Task GetMatchById_ReturnsOk_WhenMatchIsFinished()
        {
            var matchId = "match123";
            var mockMatchService = new Mock<IMatchService>();
            var mockMatchHeaderService = new Mock<IMatchHeaderService>();

            var match = new MatchWithPlayers
            {
                Id = matchId,
                Status = "Finished",
                StartDate = DateTime.UtcNow,
                RemainingPlayer = 4,
                RowNumber = 1,
                PlayerOne = new User
                {
                    Id = "user1",
                    Username = "Alice",
                    ProfilePicture = "alice.jpg"
                },
                PlayerTwo = new User
                {
                    Id = "user2",
                    Username = "Bob",
                    ProfilePicture = "bob.jpg"
                },
                PlayerOneStat = new PlayerMatchStat
                {
                    Appeared = true,
                    Won = true,
                    SetsWon = 3,
                    LegsWon = 6,
                    Averages = 91.3,
                    Max180s = 2,
                    CheckoutPercentage = 50.2,
                    HighestCheckout = 140,
                    NineDarter = 0
                },
                PlayerTwoStat = new PlayerMatchStat
                {
                    Appeared = true,
                    Won = false,
                    SetsWon = 1,
                    LegsWon = 3,
                    Averages = 85.0,
                    Max180s = 1,
                    CheckoutPercentage = 33.3,
                    HighestCheckout = 100,
                    NineDarter = 0
                }
            };

            mockMatchService.Setup(s => s.GetMatchWithPlayersByIdAsync(matchId))
                            .ReturnsAsync(match);

            var controller = new MatchController(mockMatchService.Object, mockMatchHeaderService.Object);

            var result = await controller.GetMatchById(matchId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JsonSerializer.Serialize(okResult.Value);
            var root = JsonSerializer.Deserialize<JsonElement>(json);

            Assert.Equal("Finished", root.GetProperty("Status").GetString());
            Assert.Equal("Alice", root.GetProperty("playerOne").GetProperty("Username").GetString());
            Assert.Equal("Bob", root.GetProperty("playerTwo").GetProperty("Username").GetString());
            Assert.Equal(91.3, root.GetProperty("playerOneStat").GetProperty("Averages").GetDouble());
            Assert.Equal(140, root.GetProperty("playerOneStat").GetProperty("HighestCheckout").GetInt32());
        }

        [Fact]
        public async Task GetMatchById_ReturnsNotFound_WhenMatchDoesNotExist()
        {
            // Arrange
            var matchId = "nonexistentMatch";
            var mockMatchService = new Mock<IMatchService>();
            var mockMatchHeaderService = new Mock<IMatchHeaderService>();

            _mockMatchService.Setup(s => s.GetMatchWithPlayersByIdAsync(matchId))
                            .ReturnsAsync((MatchWithPlayers?)null);

            var controller = new MatchController(mockMatchService.Object, mockMatchHeaderService.Object);

            var result = await controller.GetMatchById(matchId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            var json = JsonSerializer.Serialize(notFoundResult.Value);
            var body = JsonSerializer.Deserialize<JsonElement>(json);

            Assert.Equal($"A mérkőzés az ID-vel ({matchId}) nem található.", body.GetProperty("message").GetString());
        }

    }
}