using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UserPanel.Models;

namespace UserPanel.Services
{
    public class PasswordHasher
    {
        public class HashComparer : IEqualityComparer<byte[]>
        {
            public new bool Equals(byte[] x, byte[] y)
            {

                byte[] arrX = (byte[])x;
                byte[] arrY = (byte[])y;

                if (arrX.Length != arrY.Length)
                    return false;

                for (int i = 0; i < arrX.Length; i++)
                {
                    if (arrX[i] != arrY[i])
                        return false;
                }

                return true;
            }

            public int GetHashCode(byte[] obj)
            {
                if (obj == null)
                    throw new ArgumentNullException(nameof(obj));

                if (!(obj is byte[]))
                    throw new ArgumentException("Object must be a byte array");

                byte[] arr = (byte[])obj;

                int hash = 17;

                foreach (byte b in arr)
                {
                    hash = hash * 31 + b;
                }

                return hash;
            }
        }



        private PasswordHashOptions options;

        public PasswordHasher(IOptions<PasswordHashOptions> options)
        {
            this.options = options.Value;
        }

        public string HashPassword(string password)
        {
            byte[] saltBuffer;
            byte[] hashBuffer;
            using (var keyDerivation = new Rfc2898DeriveBytes(password, options.SaltSize, options.Iterations, options.passwordHasherAlgorithms))
            {
                saltBuffer = keyDerivation.Salt;
                hashBuffer = keyDerivation.GetBytes(options.HashSize);
            }

            byte[] result = new byte[options.HashSize + options.SaltSize];
            Buffer.BlockCopy(hashBuffer, 0, result, 0, options.HashSize);
            Buffer.BlockCopy(saltBuffer, 0, result, options.HashSize, options.SaltSize);
            return Convert.ToBase64String(result);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (!IsBase64String(hashedPassword)) return false;
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            if (hashedPasswordBytes.Length != options.HashSize + options.SaltSize)
            {
                return false;
            }

            byte[] hashBytes = new byte[options.HashSize];
            Buffer.BlockCopy(hashedPasswordBytes, 0, hashBytes, 0, options.HashSize);
            byte[] saltBytes = new byte[options.SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, options.HashSize, saltBytes, 0, options.SaltSize);

            byte[] providedHashBytes;
            using (var keyDerivation = new Rfc2898DeriveBytes(providedPassword, saltBytes, options.Iterations, options.passwordHasherAlgorithms))
            {
                providedHashBytes = keyDerivation.GetBytes(options.HashSize);
            }

            return new HashComparer().Equals(hashBytes, providedHashBytes);
        }
        public static bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }

        public string GetSaltUser(string hashedPassword)
        {
            if (!IsBase64String(hashedPassword)) 
                throw new Exception("Bad format of hashedPassword");

            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            if (hashedPasswordBytes.Length != options.HashSize + options.SaltSize)
            {
                throw new Exception("Bad format of heashedPassword");
            }
            byte[] saltBytes = new byte[options.SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, options.HashSize, saltBytes, 0, options.SaltSize);
            return Convert.ToBase64String(saltBytes);
        }

        public int OneTimeTokenGenerate(int len)
        {
            const string chars = "0123456789";
            char[] token = new char[len];
            byte[] randomBytes = new byte[len];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Mapowanie bajtów na dostępne znaki
            for (int i = 0; i < len; i++)
            {
                token[i] = chars[randomBytes[i] % chars.Length];
            }
            int code = int.Parse(String.Join("",token));

            return code;


        }

        public string HashOneTimeToken(int token, int a)
        {
            string hashed = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Math.Sin(a * token)}"));
            return hashed + ":" + a.ToString();
        }

        public bool VerifyOneTimeToken(string token, string hashed)
        {
            string[] parts = hashed.Split(":");

            int.TryParse(token, out int tokenParsed);

            if (parts.Length < 2) {  return false; }

            var binnary = Encoding.UTF8.GetBytes( Math.Sin(int.Parse(parts[1]) * tokenParsed).ToString() );

            return Convert.ToBase64String(binnary) == parts[0];
        }

    }
}
