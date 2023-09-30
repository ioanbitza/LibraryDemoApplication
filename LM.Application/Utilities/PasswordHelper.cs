using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

// This Service can be placed in LM.Infrastructure too
namespace LM.Application.Utilities
{
    public static class PasswordHelper
    {


        public static bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentNullException(nameof(passwordHash));

            // We assume that passwordHash contains both the salt and the hash itself, separated by ':'
            var parts = passwordHash.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) throw new InvalidOperationException("Invalid password hash format.");

            var salt = Convert.FromBase64String(parts[0]);
            var actualHash = Convert.FromBase64String(parts[1]);

            // We generate the hash for the entered password using the same salt
            var expectedHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
