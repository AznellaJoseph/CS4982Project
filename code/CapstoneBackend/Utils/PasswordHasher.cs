using System;
using System.Security.Cryptography;

namespace CapstoneBackend.Utils
{
    /// <summary>
    ///     Handing Hashing and Validating Passwords
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 16;
        private const int HashIterations = 1000;

        /// <summary>
        ///     Hashes a given password.
        /// </summary>
        /// <param name="password">the password to hash</param>
        /// <returns>the hashed password</returns>
        public static string Hash(string password)
        {
            var salt = new byte[SaltSize];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            var pdkdf2 = new Rfc2898DeriveBytes(password, salt, HashIterations);
            var hash = pdkdf2.GetBytes(HashSize);
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        ///     Validates a password with another password hash, in other words, checking if they are equal.
        /// </summary>
        /// <param name="password">the password to be validated</param>
        /// <param name="validPassword">the hashed password to validate with.</param>
        /// <returns>true if the password is valid; false otherwise.</returns>
        public static bool ValidatePassword(string password, string validPassword)
        {
            var validHash = Convert.FromBase64String(validPassword);
            var salt = new byte[SaltSize];
            Array.Copy(validHash, 0, salt, 0, SaltSize);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, HashIterations);
            var hash = pbkdf2.GetBytes(HashSize);
            var valid = true;
            for (var i = 0; i < HashSize; i++)
                if (validHash[i + SaltSize] != hash[i])
                    valid = false;

            return valid;
        }
    }
}