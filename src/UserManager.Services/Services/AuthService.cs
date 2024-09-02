using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using UserManager.Common.Configs;
using UserManager.Common.Exceptions;
using UserManager.Common.Interfaces.Repos;
using UserManager.Common.Interfaces.Services;
using UserManager.Common.Models.Request;
using UserManager.Repo.Entities;

namespace UserManager.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _usersRepo;
        private readonly IHashService _hashService;
        private readonly JwtConfigurations _jwtConfig;

        public AuthService(IRepository<User> usersRepo, IHashService hashService, IOptions<JwtConfigurations> config)
        {
            _usersRepo = usersRepo;
            _hashService = hashService;
            _jwtConfig = config.Value;
        }

        /// <summary>
        /// Authenticates a user based on the provided login credentials and generates a JWT token.
        /// </summary>
        /// <param name="loginRequest">An object containing the user's login credentials (username or email and password).</param>
        /// <returns>
        /// The task result contains the JWT token if authentication is successful.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="loginRequest"/> is null.
        /// </exception>
        /// <exception cref="ApiValidationException">
        /// Thrown when the credentials are invalid, resulting in unauthorized access.
        /// </exception>
        public async Task<string> AuthenticateAsync(LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                throw new ArgumentNullException(nameof(loginRequest));
            }

            // Get user and verify password
            var user = await _usersRepo.GetAsync(u => u.Username == loginRequest.UsernameOrEmail || u.Email == loginRequest.UsernameOrEmail);
            if (user == null || !_hashService.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new ApiValidationException(HttpStatusCode.Unauthorized, "Invalid username or password");
            }

            return CreateToken(user);
        }

        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the JWT token is to be created.</param>
        /// <returns>
        /// A string containing the generated JWT token.
        /// </returns>
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtConfig.Secret));

            var token = new JwtSecurityToken
                (
                    claims: claims,
                    issuer: _jwtConfig.Issuer,
                    expires: DateTime.Now.AddMinutes(_jwtConfig.TokenExpiryInMins),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
