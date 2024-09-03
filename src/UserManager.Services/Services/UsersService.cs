using System.Net;
using UserManager.Common.Exceptions;
using UserManager.Common.Extensions;
using UserManager.Common.Interfaces.Repos;
using UserManager.Common.Interfaces.Services;
using UserManager.Common.Models.Request;
using UserManager.Common.Models.Response;
using UserManager.Repo.Entities;

namespace UserManager.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<User> _usersRepo;
        private readonly IHashService _hashService;
        private readonly IRepository<UserDetails> _userDetailsRepo;

        public UsersService(IRepository<User> usersRepo, IHashService hashService, IRepository<UserDetails> userDetailsRepo)
        {
            _usersRepo = usersRepo;
            _hashService = hashService;
            _userDetailsRepo = userDetailsRepo;
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

            // Check if username or email exists
            var user = await _usersRepo.GetAsync(u => u.Username == userRequest.Username || u.Email == userRequest.Email);
            if (user != null)
            {
                throw new ApiValidationException(HttpStatusCode.BadRequest, "Username or email already exists");
            }

            // Create password hash and salt
            var (passwordHash, passwordSalt) = _hashService.CreatePasswordHash(userRequest.Password);

            // Add new user
            var newUser = new User
            {
                Username = userRequest.Username.SanitizeAndTrim(),
                Email = userRequest.Email.SanitizeAndTrim(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _usersRepo.CreateAsync(newUser);
        }

        /// <summary>
        /// Retrieves the profile details of a user by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile details are to be retrieved.</param>
        /// <returns>
        /// The task result contains a <see cref="UserDetailsResponse"/> object with the user's profile details.
        /// </returns>
        public async Task<UserDetailsResponse> GetUserProfileDetails(int userId)
        {
            var userDetails = await _userDetailsRepo.GetAsync(o => o.UserId == userId);
            return new UserDetailsResponse(userDetails);
        }

        /// <summary>
        /// Updates the profile details of a user or adds new details if they do not exist.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile details are to be updated.</param>
        /// <param name="userDetailsRequest">The object containing the updated profile details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="userDetailsRequest"/> is null.
        /// </exception>
        public async Task UpdateUserProfileDetails(int userId, UserDetailsRequest userDetailsRequest)
        {
            if (userDetailsRequest == null)
            {
                throw new ArgumentNullException(nameof(userDetailsRequest));
            }

            var userDetails = new UserDetails()
            {
                UserId = userId,
                FirstName = userDetailsRequest.FirstName.SanitizeAndTrim(),
                LastName = userDetailsRequest.LastName.SanitizeAndTrim(),
                Phone = userDetailsRequest.Phone.SanitizeAndTrim(),
                Address = userDetailsRequest.Address.SanitizeAndTrim(),
                DateOfBirth = userDetailsRequest.DateOfBirth,
            };
            await _userDetailsRepo.CreateOrUpdateAsync(userDetails, o => o.UserId == userId);
        }
    }
}
