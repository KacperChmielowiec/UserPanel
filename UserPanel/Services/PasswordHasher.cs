using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
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
    }
}
