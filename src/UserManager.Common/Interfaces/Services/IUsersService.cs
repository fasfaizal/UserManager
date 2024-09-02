using UserManager.Common.Models.Request;
using UserManager.Common.Models.Response;

namespace UserManager.Common.Interfaces.Services
{
    public interface IUsersService
    {
        Task CreateAsync(CreateUserRequest userRequest);
        Task<UserDetailsResponse> GetUserProfileDetails(int userId);
        Task UpdateUserProfileDetails(int userId, UserDetailsRequest userDetailsRequest);
    }
}
