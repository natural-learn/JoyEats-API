using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkyTakeOut.Common.Helpers
{
    public class JWTHelper
    {
        /// <summary>
        /// 创建JWT Token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <param name="secretKey">密钥</param>
        /// <param name="issuer">颁发者</param>
        /// <param name="audience">接收者</param>
        /// <param name="expireMinutes">过期时间（分钟）</param>
        /// <returns></returns>
        public static string CreateToken(
            string id, 
            string userName, 
            string role,
            string secretKey,
            string issuer,
            string audience,
            int expireMinutes)
        {
            byte[] secKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var secKey = new SymmetricSecurityKey(secKeyBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role),
            };
            var tokenDescriptor = new JwtSecurityToken(
                issuer,
                claims: claims,
                audience: audience,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
