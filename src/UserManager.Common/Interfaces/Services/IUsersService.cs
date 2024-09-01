using UserManager.Common.Models.Request;

namespace UserManager.Common.Interfaces.Services
{
    public interface IUsersService
    {
        Task CreateAsync(CreateUserRequest userRequest);
    }
}
