using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Services
{
    public sealed class HashingOptions
    {
        public int Iterations { get; set; } = 10000;
    }
    public sealed class PasswordHasher : IPasswordHasher
    {
        private HashingOptions Options { get; }

        private const int SaltSize = 16;
        private const int KeySize = 32;

        public PasswordHasher(IOptions<HashingOptions> options)
        {
            Options = options.Value;
        }

        public string HashPassword(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                  password,
                  SaltSize,
                  Options.Iterations,
                  HashAlgorithmName.SHA256))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{Options.Iterations}.{salt}.{key}";
            }
        }

        public Microsoft.AspNet.Identity.PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            Microsoft.AspNet.Identity.PasswordVerificationResult result = 0;
            var parts = hashedPassword.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format. " +
                  "Should be formatted as `{iterations}.{salt}.{hash}`");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needsUpgrade = iterations != Options.Iterations;

            using (var algorithm = new Rfc2898DeriveBytes(
              providedPassword,
              salt,
              iterations,
              HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(KeySize);

                var verified = keyToCheck.SequenceEqual(key);
                if (verified)
                {
                    result = Microsoft.AspNet.Identity.PasswordVerificationResult.Success;
                }
                else if (iterations != Options.Iterations)
                {
                    result = Microsoft.AspNet.Identity.PasswordVerificationResult.SuccessRehashNeeded;
                }
                else
                {
                    result = Microsoft.AspNet.Identity.PasswordVerificationResult.Failed;
                }
                return result;
            }
        }

    }
}
