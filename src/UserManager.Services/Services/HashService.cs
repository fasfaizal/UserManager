using UserManager.Common.Interfaces.Services;

namespace UserManager.Services.Services
{
    public class HashService : IHashService
    {
        /// <summary>
        /// Creates a password hash and salt using HMACSHA512 encryption.
        /// </summary>
        /// <param name="password">The plaintext password to be hashed.</param>
        /// <returns>
        /// A tuple containing the password hash and the password salt.
        /// </returns>
        public (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (PasswordHash: passwordHash, PasswordSalt: passwordSalt);
            }
        }

        /// <summary>
        /// Verifies a password against a stored hash and salt using HMACSHA512 encryption.
        /// </summary>
        /// <param name="password">The plaintext password to be verified.</param>
        /// <param name="passwordHash">The stored password hash to compare against.</param>
        /// <param name="passwordSalt">The stored password salt used to generate the hash.</param>
        /// <returns>
        /// <c>true</c> if the computed hash matches the stored hash; otherwise, <c>false</c>.
        /// </returns>
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
