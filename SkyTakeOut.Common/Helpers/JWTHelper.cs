using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SkyTakeOut.Common.Configs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkyTakeOut.Common.Helpers
{
    public class JWTHelper
    {
        private readonly JwtAdminSettings _jwtAdminSettings;

        public JWTHelper(IOptions<JwtAdminSettings> options)
        {
            _jwtAdminSettings = options.Value;
        }

        public string CreateToken(string id, string userName, string role)
        {
            byte[] secKeyBytes = Encoding.UTF8.GetBytes(_jwtAdminSettings.Secret);
            var secKey = new SymmetricSecurityKey(secKeyBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role),
            };
            var tokenDescriptor = new JwtSecurityToken(
                _jwtAdminSettings.Issuer,
                claims: claims,
                audience: _jwtAdminSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtAdminSettings.ExpireMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
