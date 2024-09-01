using Microsoft.EntityFrameworkCore;

namespace UserManager.Repo.DbContexts
{
    public class UserManagerDbContext : DbContext
    {
        public UserManagerDbContext(DbContextOptions<UserManagerDbContext> options) : base(options) { }
    }
}
