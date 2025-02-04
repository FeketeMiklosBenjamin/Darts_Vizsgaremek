using System.Security.Claims;
using Vizsga_Backend.Services;

namespace Vizsga_Backend.Middlewares
{
    public class ActivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SessionStore _sessionStore;

        public ActivityMiddleware(RequestDelegate next, SessionStore sessionStore)
        {
            _next = next;
            _sessionStore = sessionStore;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                if (context.User.Identity!.IsAuthenticated)
                {
                    var sessionId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (sessionId == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("No session ID found in claims.");
                        return;
                    }

                    if (!await _sessionStore.IsSessionActiveAsync(sessionId))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Session expired due to inactivity.");
                        return;
                    }

                    await _sessionStore.UpdateLastActivityAsync(sessionId);
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                // Log a hiba részleteit
                Console.Error.WriteLine($"Error in ActivityMiddleware: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An error occurred while processing your request.");
            }
        }
    }
}
