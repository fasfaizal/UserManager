using System.Net;
using UserManager.Common.Exceptions;
using UserManager.Common.Interfaces.Repos;
using UserManager.Common.Interfaces.Services;
using UserManager.Common.Models.Request;
using UserManager.Common.Models.Response;
using UserManager.Repo.Entities;

namespace UserManager.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepo _usersRepo;
        private readonly IHashService _hashService;
        private readonly IUserDetailsRepo _userDetailsRepo;

        public UsersService(IUsersRepo usersRepo, IHashService hashService, IUserDetailsRepo userDetailsRepo)
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

            var user = await _usersRepo.GetByUsernameOrEmailAsync(userRequest.Username.Trim(), userRequest.Email.Trim());
            if (user != null)
            {
                throw new ApiValidationException(HttpStatusCode.BadRequest, "Username or email already exists");
            }

            var (passwordHash, passwordSalt) = _hashService.CreatePasswordHash(userRequest.Password);
            var newUser = new User
            {
                Username = userRequest.Username.Trim(),
                Email = userRequest.Email.Trim(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _usersRepo.AddAsync(newUser);
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
            var userDetails = await _userDetailsRepo.GetUserDetailsAsync(userId);
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

            UserDetails userDetails = await _userDetailsRepo.GetUserDetailsAsync(userId);
            if (userDetails == null)
            {
                userDetails = new UserDetails()
                {
                    UserId = userId,
                    FirstName = userDetailsRequest.FirstName,
                    LastName = userDetailsRequest.LastName,
                    Phone = userDetailsRequest.Phone,
                    Address = userDetailsRequest.Address,
                    DateOfBirth = userDetailsRequest.DateOfBirth,
                };
                await _userDetailsRepo.AddUserDetailsAsync(userDetails);
            }
            else
            {
                userDetails.FirstName = userDetailsRequest.FirstName;
                userDetails.LastName = userDetailsRequest.LastName;
                userDetails.Phone = userDetailsRequest.Phone;
                userDetails.Address = userDetailsRequest.Address;
                userDetails.DateOfBirth = userDetailsRequest.DateOfBirth;
                userDetails.UpdatedOn = DateTime.Now;
                await _userDetailsRepo.SaveChangesAsync();
            }
        }
    }
}
