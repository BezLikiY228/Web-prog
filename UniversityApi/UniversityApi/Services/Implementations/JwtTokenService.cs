using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniversityApi.Configuration;
using UniversityApi.Identity;

namespace UniversityApi.Services.Implementations
{
    public class JwtTokenService
    {
        private readonly JwtSettings _settings;
        private readonly string _secretKey;

        public JwtTokenService(
            IOptions<JwtSettings> settings,
            IConfiguration configuration)
        {
            _settings = settings.Value;

            _secretKey = configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException(
                    "JWT SecretKey не настроен.");
        }

        public (string Token, DateTime ExpiresAt) CreateToken(
            ApplicationUser user,
            IList<string> roles)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(
                _settings.AccessTokenMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(
                    ClaimTypes.Name,
                    user.UserName ?? user.Email ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return (
                new JwtSecurityTokenHandler().WriteToken(token),
                expiresAt
            );
        }
    }
}