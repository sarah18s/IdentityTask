using IdentityTask.Application.Services.Interface;
using IdentityTask.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityTask.Application.Services.implementation
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        public JwtTokenService(IConfiguration config) => _config = config;
        public string CreateToken(User user, IEnumerable<string> roles)
        {
            var jwt = _config.GetSection("Jwt");
            var key = jwt.GetValue<string>("Key") ?? throw new InvalidOperationException("JWT Key missing");
            var issuer = jwt.GetValue<string>("Issuer");
            var audience = jwt.GetValue<string>("Audience");
            var expiresMinutes = jwt.GetValue<int>("ExpiresMinutes");

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
    };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: System.DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
