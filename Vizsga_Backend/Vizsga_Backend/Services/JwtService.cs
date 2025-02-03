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

        // Konstruktor, ahol beolvassuk a konfigurációt (pl. titkos kulcs, issuer, audience)
        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration.GetValue<string>("Jwt:SecretKey")!;
            _issuer = configuration.GetValue<string>("Jwt:Issuer")!;
            _audience = configuration.GetValue<string>("Jwt:Audience")!;
        }

        // Token generálása
        public string GenerateToken(string userId, string email, int role)
        {
            // Claims: a felhasználó adatai, amiket a tokenben tárolunk
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role.ToString())
        };

            // Titkos kulcs
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // JWT token létrehozása
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),  // Lejárati idő
                signingCredentials: creds
            );

            // Token string visszaadása
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
