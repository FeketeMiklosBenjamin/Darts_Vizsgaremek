using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VizsgaBackend.Services
{
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly Random _random = new Random();

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration.GetValue<string>("Jwt:SecretKey")!;
            _issuer = configuration.GetValue<string>("Jwt:Issuer")!;
            _audience = configuration.GetValue<string>("Jwt:Audience")!;
        }

        // Access token generálása
        public string GenerateToken(string userId, string email, int role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), // 15 perces érvényesség
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Refresh token generálása (visszaad egy hosszú élettartamú token-t)
        public string GenerateRefreshToken(string userId)
        {
            // A refresh token egy egyedi, véletlenszerű string, nem tárolunk róla adatokat.
            var refreshToken = GenerateRandomString(64); // 64 karakter hosszú

            // Itt nem szükséges JWT-t generálni, mert nem tárolunk benne adatokat, hanem egy egyedi azonosítót
            return refreshToken;
        }

        // Véletlenszerű string generálása (pl. 64 karakter hosszú)
        private string GenerateRandomString(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var randomString = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                randomString.Append(validChars[_random.Next(validChars.Length)]);
            }

            return randomString.ToString();
        }
    }

}
