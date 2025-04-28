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
    public class TournamentControllerTests
    {
        private readonly Mock<IMatchService> _mockMatchService;
        private readonly Mock<IMatchHeaderService> _mockMatchHeaderService;
        private readonly Mock<IOptions<MongoDBSettings>> _mockMongoDbSettings;
        private readonly IConfiguration _configuration;
        private readonly Mock<HttpContext> _mockHttpContext;

        private readonly TournamentController _controller;

        public TournamentControllerTests()
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

            _controller = new TournamentController(
                _mockMatchHeaderService.Object,
                _mockMatchService.Object
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
        public async Task GetAllTournamentHeader_ReturnsOkWithTournaments()
        {
            var tournaments = new List<MatchHeader>
            {
                new MatchHeader { Id = "1", Name = "Teszt Verseny", Level = "A", BackroundImageUrl = "img.png", TournamentStartDate = DateTime.UtcNow, TournamentEndDate = DateTime.UtcNow.AddDays(1) }
            };

            var mockMatchHeaderService = new Mock<IMatchHeaderService>();
            mockMatchHeaderService.Setup(x => x.GetAllDrawedTournamentAsync()).ReturnsAsync(tournaments);

            var controller = new TournamentController(mockMatchHeaderService.Object, Mock.Of<IMatchService>());
            var result = await controller.GetAllTournamentHeader();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Single((IEnumerable<object>)okResult.Value);
        }

        [Fact]
        public async Task GetAllTournamentHeader_WhenExceptionThrown_ReturnsInternalServerError()
        {
            var mockMatchHeaderService = new Mock<IMatchHeaderService>();
            mockMatchHeaderService.Setup(x => x.GetAllDrawedTournamentAsync()).ThrowsAsync(new Exception());

            var controller = new TournamentController(mockMatchHeaderService.Object, Mock.Of<IMatchService>());
            var result = await controller.GetAllTournamentHeader();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetTournamentWithMatches_ValidId_ReturnsCorrectMatchData()
        {
            var match1 = new MatchWithPlayers
            {
                Id = "match1234",
                Status = "Finished",
                StartDate = DateTime.UtcNow,
                RemainingPlayer = 4,
                RowNumber = 1,
                PlayerOne = new User { Id = "user1", Username = "Alice", ProfilePicture = "alice.jpg" },
                PlayerTwo = new User { Id = "user2", Username = "Bob", ProfilePicture = "bob.jpg" },
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

            var match2 = new MatchWithPlayers
            {
                Id = "match5678",
                Status = "Finished",
                StartDate = DateTime.UtcNow,
                RemainingPlayer = 2,
                RowNumber = 2,
                PlayerOne = new User { Id = "user3", Username = "Charlie", ProfilePicture = "charlie.jpg" },
                PlayerTwo = new User { Id = "user4", Username = "Dave", ProfilePicture = "dave.jpg" },
                PlayerOneStat = new PlayerMatchStat
                {
                    Appeared = true,
                    Won = false,
                    SetsWon = 2,
                    LegsWon = 5,
                    Averages = 89.1,
                    Max180s = 1,
                    CheckoutPercentage = 45.7,
                    HighestCheckout = 120,
                    NineDarter = 0
                },
                PlayerTwoStat = new PlayerMatchStat
                {
                    Appeared = true,
                    Won = true,
                    SetsWon = 3,
                    LegsWon = 6,
                    Averages = 92.4,
                    Max180s = 3,
                    CheckoutPercentage = 55.3,
                    HighestCheckout = 160,
                    NineDarter = 0
                }
            };

            var header = new MatchHeaderWithMatches
            {
                Id = "header1",
                Name = "Tesztverseny",
                Level = "A",
                IsDrawed = true,
                TournamentStartDate = DateTime.UtcNow,
                TournamentEndDate = DateTime.UtcNow.AddDays(1),
                SetsCount = 3,
                LegsCount = 5,
                StartingPoint = 501,
                Matches = new List<MatchWithPlayers> { match1, match2 }
            };

            var mockHeaderService = new Mock<IMatchHeaderService>();
            var mockMatchService = new Mock<IMatchService>();

            mockHeaderService.Setup(x => x.GetTournamentWithMatchesAsync("header1")).ReturnsAsync(header);

            var controller = new TournamentController(mockHeaderService.Object, mockMatchService.Object);
            var result = await controller.GetTournamentWithMatches("header1");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JsonSerializer.Serialize(okResult.Value);
            var root = JsonSerializer.Deserialize<JsonElement>(json);

            Console.WriteLine(json);

            var matches = root.GetProperty("matches").EnumerateArray().ToList();

            var match1Json = matches[0];
            Assert.Equal("Finished", match1Json.GetProperty("Status").GetString());
            Assert.Equal("Alice", match1Json.GetProperty("playerOne").GetProperty("Username").GetString());
            Assert.Equal("Bob", match1Json.GetProperty("playerTwo").GetProperty("Username").GetString());
            Assert.Equal(3, match1Json.GetProperty("playerOneResult").GetInt32());
            Assert.Equal(1, match1Json.GetProperty("playerTwoResult").GetInt32());

            var match2Json = matches[1];
            Assert.Equal("Finished", match2Json.GetProperty("Status").GetString());
            Assert.Equal("Charlie", match2Json.GetProperty("playerOne").GetProperty("Username").GetString());
            Assert.Equal("Dave", match2Json.GetProperty("playerTwo").GetProperty("Username").GetString());
            Assert.Equal(2, match2Json.GetProperty("playerOneResult").GetInt32());
            Assert.Equal(3, match2Json.GetProperty("playerTwoResult").GetInt32());
        }
    }
}
