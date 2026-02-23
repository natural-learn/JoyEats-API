using System.Security.Cryptography;

namespace JoyEats.Common
{
    /// <summary>
    /// 密码帮助类
    /// </summary>
    public class PasswordHelper
    {
        /// <summary>
        /// 使用带盐的SHA256算法对密码进行哈希处理
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            RandomNumberGenerator.Create().GetBytes(salt);  // 生成随机盐值
            byte[] hash  = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, 10000, HashAlgorithmName.SHA256, 32); // 使用PBKDF2算法进行哈希处理
            return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash); // 返回盐值和哈希值的组合
        }

        /// <summary>
        /// 密码验证
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('|');
            if (parts.Length != 2)
                return false;
            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, 10000, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
        }
    }
}
