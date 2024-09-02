using Moq;
using System.Net;
using UserManager.Common.Exceptions;
using UserManager.Common.Interfaces.Repos;
using UserManager.Common.Interfaces.Services;
using UserManager.Common.Models.Request;
using UserManager.Repo.Entities;
using UserManager.Services.Services;

namespace UserManager.Services.Tests.Services
{
    public class UsersServiceTests
    {
        private readonly UsersService _userService;
        private readonly Mock<IUsersRepo> _usersRepoMock;
        private readonly Mock<IHashService> _hashServiceMock;
        private readonly Mock<IUserDetailsRepo> _userDetailsRepoMock;

        public UsersServiceTests()
        {
            _usersRepoMock = new Mock<IUsersRepo>();
            _hashServiceMock = new Mock<IHashService>();
            _userDetailsRepoMock = new Mock<IUserDetailsRepo>();
            _userService = new UsersService(_usersRepoMock.Object, _hashServiceMock.Object, _userDetailsRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_NullUserRequest_ThrowsArgumentNullException()
        {
            // Arrange
            CreateUserRequest userRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.CreateAsync(userRequest));
        }

        [Fact]
        public async Task CreateAsync_UserAlreadyExists_ThrowsApiValidationException()
        {
            // Arrange
            var userRequest = new CreateUserRequest { Username = "existingUser", Email = "existingEmail@example.com", Password = "password123" };
            var existingUser = new User { Username = "existingUser", Email = "existingEmail@example.com" };

            _usersRepoMock.Setup(repo => repo.GetByUsernameOrEmailAsync(userRequest.Username, userRequest.Email))
                .ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiValidationException>(() => _userService.CreateAsync(userRequest));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal("Username or email already exists", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ValidUserRequest_CreatesUser()
        {
            // Arrange
            var userRequest = new CreateUserRequest { Username = "newUser", Email = "newEmail@example.com", Password = "password123" };
            _usersRepoMock.Setup(repo => repo.GetByUsernameOrEmailAsync(userRequest.Username, userRequest.Email))
                .ReturnsAsync((User)null);

            var passwordHash = new byte[] { 1, 2, 3 };
            var passwordSalt = new byte[] { 4, 5, 6 };
            _hashServiceMock.Setup(service => service.CreatePasswordHash(userRequest.Password))
                .Returns((passwordHash, passwordSalt));

            _usersRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            await _userService.CreateAsync(userRequest);

            // Assert
            _usersRepoMock.Verify(repo => repo.AddAsync(It.Is<User>(u =>
                u.Username == userRequest.Username &&
                u.Email == userRequest.Email &&
                u.PasswordHash == passwordHash &&
                u.PasswordSalt == passwordSalt
            )), Times.Once);
        }
    }
}
