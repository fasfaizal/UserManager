using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManager.Repo.DbContexts;

namespace UserManager.Repo.Extensions
{
    public static class DbExtensions
    {
        public static IServiceCollection AddDBService(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UserManagerDbContext>(x => x.UseSqlServer(connectionString));
            return services;
        }
    }
}
