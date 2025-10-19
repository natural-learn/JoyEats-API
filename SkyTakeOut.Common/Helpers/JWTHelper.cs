using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkyTakeOut.Common.Helpers
{
    public class JWTHelper
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly int _expireMinutes;

        public JWTHelper(string secretKey, string issuer, int expireMinutes)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _expireMinutes = expireMinutes;
        }

        public string CreateToken(string id, string role)
        {
            byte[] secKeyBytes = Encoding.UTF8.GetBytes(_secretKey);
            var secKey = new SymmetricSecurityKey(secKeyBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                new Claim("id", id),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var tokenDescriptor = new JwtSecurityToken(
                _issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
