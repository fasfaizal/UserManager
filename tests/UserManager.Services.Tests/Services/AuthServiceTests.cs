using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using UserManager.Common.Configs;
using UserManager.Common.Exceptions;
using UserManager.Common.Interfaces.Repos;
using UserManager.Common.Interfaces.Services;
using UserManager.Common.Models.Request;
using UserManager.Repo.Entities;
using UserManager.Services.Services;

namespace UserManager.Services.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly Mock<IUsersRepo> _usersRepoMock;
        private readonly Mock<IHashService> _hashServiceMock;
        private readonly Mock<IOptions<JwtConfigurations>> _jwtConfigMock;

        public AuthServiceTests()
        {
            _usersRepoMock = new Mock<IUsersRepo>();
            _hashServiceMock = new Mock<IHashService>();
            _jwtConfigMock = new Mock<IOptions<JwtConfigurations>>();

            var jwtConfig = new JwtConfigurations
            {
                Secret = "akIGTvMErUoWFNfmcUUFyFaDtOSPvvpCakIGTvMErUoWFNfmcUUFyFaDtOSPvvpC",
                Issuer = "issuer",
                TokenExpiryInMins = 5
            };
            _jwtConfigMock.Setup(config => config.Value).Returns(jwtConfig);

            _authService = new AuthService(_usersRepoMock.Object, _hashServiceMock.Object, _jwtConfigMock.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_NullLoginRequest_ThrowsArgumentNullException()
        {
            // Arrange
            LoginRequest loginRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authService.AuthenticateAsync(loginRequest));
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidCredentials_ThrowsApiValidationException()
        {
            // Arrange
            var loginRequest = new LoginRequest { UsernameOrEmail = "user@example.com", Password = "password" };
            _usersRepoMock.Setup(repo => repo.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail, loginRequest.UsernameOrEmail))
                .ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiValidationException>(() => _authService.AuthenticateAsync(loginRequest));
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
            Assert.Equal("Invalid username or password", exception.Message);
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsJwtToken()
        {
            // Arrange
            var loginRequest = new LoginRequest { UsernameOrEmail = "user@example.com", Password = "password" };
            var user = new User
            {
                Username = "user",
                Email = "user@example.com",
                PasswordHash = new byte[] { 1, 2, 3 },
                PasswordSalt = new byte[] { 4, 5, 6 }
            };

            _usersRepoMock.Setup(repo => repo.GetByUsernameOrEmailAsync(loginRequest.UsernameOrEmail, loginRequest.UsernameOrEmail))
                .ReturnsAsync(user);
            _hashServiceMock.Setup(service => service.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
                .Returns(true);

            // Act
            var token = await _authService.AuthenticateAsync(loginRequest);

            // Assert
            Assert.NotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.Equal("user", jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal("user@example.com", jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        }
    }
}
