using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Vizsga_Backend.Controllers;
using Vizsga_Backend.Interfaces;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.MessageModels;
using Vizsga_Backend.Models.UserModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Models;

namespace Backend.Tests.ControllerTests
{
    public class MessageControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<IOptions<MongoDBSettings>> _mockMongoDbSettings;
        private readonly IConfiguration _configuration;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly MessageController _controller;

        public MessageControllerTests()
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

            _mockUserService = new Mock<IUserService>();
            _mockMessageService = new Mock<IMessageService>();

            _controller = new MessageController(
                _mockMessageService.Object,
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
        public async Task SendAdminMessage_ValidRequest_ReturnsCreated()
        {
            var adminId = "admin123";
            var adminEmail = "admin@test.com";
            var role = "2";

            SetupAuthenticatedUser(adminId, adminEmail, role);

            var messageData = new MessageSendAdmin
            {
                Title = "Fontos értesítés",
                Text = "Kérjük, frissítsd az adataidat",
                EmailAddress = "user@test.com"
            };

            var targetUser = new User { Id = "user123", EmailAddress = "user@test.com" };

            _mockUserService.Setup(x => x.GetUserByEmailAsync(messageData.EmailAddress))
                .ReturnsAsync(targetUser);
            _mockMessageService.Setup(x => x.CreateAsync(It.IsAny<Message>()))
                .Returns(Task.CompletedTask);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            var result = await _controller.SendAdminMessage(messageData);

            Assert.IsType<CreatedResult>(result);

            _mockMessageService.Verify(x => x.CreateAsync(It.Is<Message>(m =>
                m.Title == "Fontos értesítés" &&
                m.Text == "Kérjük, frissítsd az adataidat" &&
                m.FromId == null &&
                m.ToId == "user123"
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteMessage_AdminUser_DeletesAnyMessage()
        {
            var adminId = "admin123";
            var adminEmail = "admin@test.com";
            var role = "2";

            SetupAuthenticatedUser(adminId, adminEmail, role);

            var messageId = "msg123";
            var existingMessage = new Message
            {
                Id = messageId,
                ToId = "user123",
                FromId = "otherUser"
            };

            _mockMessageService.Setup(x => x.GetMessageAsync(null, messageId))
                .ReturnsAsync(existingMessage);
            _mockMessageService.Setup(x => x.DeleteAsync(messageId))
                .Returns(Task.CompletedTask);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            var result = await _controller.DeleteMessage(messageId);

            Assert.IsType<NoContentResult>(result);

            _mockMessageService.Verify(x => x.DeleteAsync(messageId), Times.Once);
        }

        [Fact]
        public async Task DeleteMessage_RegularUser_DeletesOwnMessage()
        {
            var userId = "user123";
            var email = "user@test.com";
            var role = "1";

            SetupAuthenticatedUser(userId, email, role);

            var messageId = "msg123";
            var existingMessage = new Message
            {
                Id = messageId,
                ToId = userId,
                FromId = "otherUser"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            _mockMessageService.Setup(x => x.GetMessageAsync(userId, messageId))
                .ReturnsAsync(existingMessage);
            _mockMessageService.Setup(x => x.DeleteAsync(messageId))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteMessage(messageId);

            Assert.IsType<NoContentResult>(result);

            _mockMessageService.Verify(x => x.DeleteAsync(messageId), Times.Once);
        }

        [Fact]
        public async Task SendUserMessage_ValidRequest_ReturnsCreated()
        {
            var userId = "user123";
            var userEmail = "user@test.com";
            var role = "1";

            SetupAuthenticatedUser(userId, userEmail, role);

            var message = new Message
            {
                Title = "Segítségkérés",
                Text = "Nem tudok bejelentkezni"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            _mockMessageService.Setup(x => x.CreateAsync(It.IsAny<Message>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.SendUserMessage(message);

            Assert.IsType<CreatedResult>(result);

            _mockMessageService.Verify(x => x.CreateAsync(It.Is<Message>(m =>
                m.Title == "Segítségkérés" &&
                m.Text == "Nem tudok bejelentkezni" &&
                m.FromId == userId &&
                m.ToId == null &&
                m.SendDate.Date == DateTime.UtcNow.Date
            )), Times.Once);
        }

        [Fact]
        public async Task SendUserMessage_AdminUser_ReturnsUnauthorized()
        {
            var adminId = "admin123";
            var adminEmail = "admin@test.com";
            var role = "2";

            SetupAuthenticatedUser(adminId, adminEmail, role);

            var message = new Message
            {
                Title = "Admin próbálkozik",
                Text = "Nem kéne működnie"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            var result = await _controller.SendUserMessage(message);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = JsonSerializer.Deserialize<dynamic>(JsonSerializer.Serialize(unauthorizedResult.Value));

            Assert.Equal("Csak felhasználó küldhet üzenetet az adminnak!", response.GetProperty("message").GetString());

            _mockMessageService.Verify(x => x.CreateAsync(It.IsAny<Message>()), Times.Never);
        }
    }
}
