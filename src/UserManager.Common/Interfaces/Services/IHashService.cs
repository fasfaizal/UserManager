namespace UserManager.Common.Interfaces.Services
{
    public interface IHashService
    {
        (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
