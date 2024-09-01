using Microsoft.EntityFrameworkCore;
using UserManager.Repo.Entities;

namespace UserManager.Repo.DbContexts
{
    public class UserManagerDbContext : DbContext
    {
        public UserManagerDbContext(DbContextOptions<UserManagerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
    }
}
