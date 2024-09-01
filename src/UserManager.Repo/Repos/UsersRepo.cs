using Microsoft.EntityFrameworkCore;
using UserManager.Common.Interfaces.Repos;
using UserManager.Repo.DbContexts;
using UserManager.Repo.Entities;

namespace UserManager.Repo.Repos
{
    public class UsersRepo : IUsersRepo
    {
        private readonly UserManagerDbContext _dbContext;
        public UsersRepo(UserManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds a new user to the database and saves the changes asynchronously.
        /// </summary>
        /// <param name="user">The user object to be added to the database.</param>
        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a user by their username or email address.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains 
        /// the <see cref="User"/> object if a user with the specified username or email exists;
        /// otherwise, <c>null</c>.
        /// </returns>
        public async Task<User> GetByUsernameOrEmailAsync(string username, string email)
        {
            return await _dbContext.Users
            .Where(u => u.Username == username || u.Email == email)
            .FirstOrDefaultAsync();
        }
    }
}
