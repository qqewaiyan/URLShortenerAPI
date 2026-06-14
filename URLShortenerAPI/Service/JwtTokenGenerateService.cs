using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace POS_API.Services
{
    public class JwtTokenGenerateService
    {
        private readonly IConfiguration _config;

        public JwtTokenGenerateService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(int userId)
        {
            var secret = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key is missing from configuration.");

            var key = Encoding.UTF8.GetBytes(secret);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])
                ),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = credentials
            });

            return token;
        }
    }
}