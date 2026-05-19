using System;
using System.Security.Cryptography;

namespace BSMART
{
    internal static class BsmartPasswordService
    {
        private const int Iterations = 100_000;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const string Prefix = "pbkdf2";

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Pbkdf2(password, salt, Iterations);
            return $"{Prefix}${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string password, string storedPassword)
        {
            if (string.IsNullOrEmpty(storedPassword))
                return false;

            string[] parts = storedPassword.Split('$');
            if (parts.Length == 4 && parts[0] == Prefix && int.TryParse(parts[1], out int iterations))
            {
                try
                {
                    byte[] salt = Convert.FromBase64String(parts[2]);
                    byte[] expected = Convert.FromBase64String(parts[3]);
                    byte[] actual = Pbkdf2(password, salt, iterations);
                    return CryptographicOperations.FixedTimeEquals(actual, expected);
                }
                catch
                {
                    return false;
                }
            }

            // Legacy support: old database rows stored plain text passwords.
            return string.Equals(password, storedPassword, StringComparison.Ordinal);
        }

        public static bool NeedsUpgrade(string storedPassword)
        {
            return string.IsNullOrWhiteSpace(storedPassword) ||
                !storedPassword.StartsWith(Prefix + "$", StringComparison.Ordinal);
        }

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations)
        {
            return Rfc2898DeriveBytes.Pbkdf2(
                password ?? "",
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                HashSize);
        }
    }
}
