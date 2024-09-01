using System.Net;
using UserManager.Common.Exceptions;
using UserManager.Common.Interfaces.Repos;
using UserManager.Common.Interfaces.Services;
using UserManager.Common.Models.Request;
using UserManager.Repo.Entities;

namespace UserManager.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepo _usersRepo;
        private readonly IHashService _hashService;

        public UsersService(IUsersRepo usersRepo, IHashService hashService)
        {
            _usersRepo = usersRepo;
            _hashService = hashService;
        }

        /// <summary>
        /// Creates a new user based on the provided user request. This method checks if the provided username or email already exists in the system.
        /// If they do not exist, it creates a new user with a hashed password and saves the user to the repository.
        /// </summary>
        /// <param name="userRequest">The request containing the user's details for registration.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="userRequest"/> is null.
        /// </exception>
        /// <exception cref="ApiValidationException">
        /// Thrown when the username or email already exists.
        /// </exception>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateAsync(CreateUserRequest userRequest)
        {
            if (userRequest == null)
            {
                throw new ArgumentNullException(nameof(userRequest));
            }

            var user = await _usersRepo.GetByUsernameOrEmailAsync(userRequest.Username, userRequest.Email);
            if (user != null)
            {
                throw new ApiValidationException(HttpStatusCode.BadRequest, "Username or email already exists");
            }

            var (passwordHash, passwordSalt) = _hashService.CreatePasswordHash(userRequest.Password);
            var newUser = new User
            {
                Username = userRequest.Username,
                Email = userRequest.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _usersRepo.AddAsync(newUser);
        }
    }
}
