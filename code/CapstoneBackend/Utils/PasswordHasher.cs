using System;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;

namespace CapstoneBackend.Utils
{
    public static class PasswordHasher
    {

        private const int SaltSize = 16;
        private const int HashSize = 16;
        private const int HashIterations = 1000;

        public static string Hash(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            var pdkdf2 = new Rfc2898DeriveBytes(password, salt, HashIterations);
            var hash = pdkdf2.GetBytes(HashSize);
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }
    }
}