using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson.IO;
using System.Text.Json;
using Vizsga_Backend.Models;

namespace Vizsga_Backend.Services
{
    public class SessionStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SetSessionAsync(string sessionId, string userId, int userRole)
        {
            var sessionData = new SessionData
            {
                UserId = userId,
                UserRole = userRole,
                LastActivity = DateTime.UtcNow
            };

            var sessionJson = JsonSerializer.Serialize(sessionData);

            // Set cookie with session data
            _httpContextAccessor.HttpContext.Response.Cookies.Append(sessionId, sessionJson, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(15),  // Cookie expiry time (15 minutes)
                HttpOnly = true,  // Security option, cookie can't be accessed via JavaScript
                Secure = true,    // Set to true if using HTTPS
                SameSite = SameSiteMode.Strict  // Additional security for cross-site requests
            });
        }

        public async Task<SessionData?> GetSessionAsync(string sessionId)
        {
            var sessionJson = _httpContextAccessor.HttpContext.Request.Cookies[sessionId];
            return sessionJson != null ? JsonSerializer.Deserialize<SessionData>(sessionJson) : null;
        }

        public async Task UpdateLastActivityAsync(string sessionId)
        {
            var sessionData = await GetSessionAsync(sessionId);
            if (sessionData != null)
            {
                sessionData.LastActivity = DateTime.UtcNow;
                await SetSessionAsync(sessionId, sessionData.UserId!, sessionData.UserRole!);
            }
        }

        public async Task<bool> IsSessionActiveAsync(string sessionId)
        {
            var sessionData = await GetSessionAsync(sessionId);
            if (sessionData == null)
                return false;

            var lastActivity = DateTime.Parse(sessionData.LastActivity.ToString());
            return (DateTime.UtcNow - lastActivity).TotalMinutes <= 15; // Aktivitási idő ellenőrzése
        }
    }

}
