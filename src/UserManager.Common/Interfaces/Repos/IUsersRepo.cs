using UserManager.Repo.Entities;

namespace UserManager.Common.Interfaces.Repos
{
    public interface IUsersRepo
    {
        Task AddAsync(User user);
        Task<User> GetByUsernameOrEmailAsync(string username, string email);
    }
}
