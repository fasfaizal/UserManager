using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserManager.Common.Interfaces.Repos;
using UserManager.Repo.DbContexts;

namespace UserManager.Repo.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly UserManagerDbContext _context;

        public Repository(UserManagerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves an entity from the database that matches the specified predicate asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entity to retrieve.</typeparam>
        /// <param name="predicate">An expression that defines the condition to match the entity.</param>
        /// <returns>
        /// The task result contains the entity if found, otherwise <c>null</c>.
        /// </returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Adds a new entity to the database and saves the changes asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entity to add.</typeparam>
        /// <param name="entity">The entity to be added to the database.</param>
        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Creates a new entity or updates an existing entity in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entity to create or update.</typeparam>
        /// <param name="entity">The entity to be created or updated.</param>
        /// <param name="predicate">An expression that defines the condition to find an existing entity.</param>
        /// <remarks>
        /// This method first checks if an entity that matches the specified predicate exists in the database.
        /// If an existing entity is found, it updates the entity's values with the provided entity values.
        /// If no matching entity is found, it adds the provided entity to the database.
        /// </remarks>
        public async Task<T> CreateOrUpdateAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            var existingEntity = await _context.Set<T>().FirstOrDefaultAsync(predicate);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                await _context.Set<T>().AddAsync(entity);
            }

            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
