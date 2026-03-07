using Carely.Dtos.Requests.Jwt;
using Carely.Dtos.Responses.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Carely.Services
{
    public interface IJwtTokenProvider
    {
        public TokenResponse GenerateJwtToken(GenerateTokenRequest request);
    }
    public class JwtTokenProvider(IConfiguration configuration) : IJwtTokenProvider
    {
        public TokenResponse GenerateJwtToken(GenerateTokenRequest request)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;
            var key = jwtSettings["SecretKey"]!;
            var expires = DateTime.Now.AddMinutes(int.Parse(jwtSettings["TokenExpirationInMinutes"]!));

            var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, request.Id!),
            new (ClaimTypes.NameIdentifier, request.Id!),
            new (JwtRegisteredClaimNames.Email, request.Email!),
            new (JwtRegisteredClaimNames.FamilyName, request.LastName!),
            new (JwtRegisteredClaimNames.GivenName, request.FirstName!),
        };

            foreach (var role in request.Roles)
                claims.Add(new(ClaimTypes.Role, role));

            foreach (var permission in request.Permissions)
                claims.Add(new("permission", permission));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);

            return new TokenResponse
            {
                AccessToken = tokenHandler.WriteToken(securityToken),
                RefreshToken = "7a6f23b4e1d04c9a8f5b6d7c8a9e01f1",
                Expires = expires
            };
        }
    }
}
