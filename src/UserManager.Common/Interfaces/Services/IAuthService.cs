using UserManager.Common.Models.Request;
using UserManager.Repo.Entities;

namespace UserManager.Common.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(LoginRequest loginRequest);
        string CreateToken(User user);
    }
}
