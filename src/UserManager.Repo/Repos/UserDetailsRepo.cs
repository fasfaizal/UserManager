using Microsoft.EntityFrameworkCore;
using UserManager.Common.Interfaces.Repos;
using UserManager.Repo.DbContexts;
using UserManager.Repo.Entities;

namespace UserManager.Repo.Repos
{
    public class UserDetailsRepo : IUserDetailsRepo
    {
        private readonly UserManagerDbContext _dbContext;

        public UserDetailsRepo(UserManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves the user details for a specific user ID from the database.
        /// </summary>
        /// <param name="userId">The ID of the user whose details are to be retrieved.</param>
        /// <returns>
        /// The task result contains the <see cref="UserDetails"/> object if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<UserDetails> GetUserDetailsAsync(int userId)
        {
            return await _dbContext.UserDetails.FirstOrDefaultAsync(o => o.UserId == userId);
        }

        /// <summary>
        /// Adds new user details to the database and saves the changes asynchronously.
        /// </summary>
        /// <param name="userDetails">The user details object to be added to the database.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>

        public async Task AddUserDetailsAsync(UserDetails userDetails)
        {
            await _dbContext.UserDetails.AddAsync(userDetails);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Saves all changes made in the current database context asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
