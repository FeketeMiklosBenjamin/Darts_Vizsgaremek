namespace VizsgaBackend.Services
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email, int role);
        string GenerateRefreshToken();
    }
}
