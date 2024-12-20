using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace UserPanel.Services
{
    public static class TokenHasher
    { 
        public static string HashToken(string secret, string data)
        {
            byte[] hashBytes;
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
               hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
            return Convert.ToBase64String(hashBytes);
        }

        public static byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];

            // Wypełnia tablicę losowymi bajtami
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return randomBytes;
        }

        
    }
}
