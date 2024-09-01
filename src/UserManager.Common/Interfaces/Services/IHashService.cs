namespace UserManager.Common.Interfaces.Services
{
    public interface IHashService
    {
        (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password);
    }
}
