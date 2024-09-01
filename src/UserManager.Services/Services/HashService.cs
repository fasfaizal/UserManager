using UserManager.Common.Interfaces.Services;

namespace UserManager.Services.Services
{
    public class HashService : IHashService
    {
        public (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (PasswordHash: passwordHash, PasswordSalt: passwordSalt);
            }
        }
    }
}
