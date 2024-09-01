using UserManager.Services.Services;

namespace UserManager.Services.Tests.Services
{
    public class HashServiceTests
    {
        private readonly HashService _hashService;

        public HashServiceTests()
        {
            _hashService = new HashService();
        }

        [Fact]
        public void CreatePasswordHash_SamePasswordDifferentHash_ReturnsDifferentHashes()
        {
            // Arrange
            string password = "TestPassword123";

            // Act
            var result1 = _hashService.CreatePasswordHash(password);
            var result2 = _hashService.CreatePasswordHash(password);

            // Assert
            Assert.NotEqual(result1.PasswordHash, result2.PasswordHash);
            Assert.NotEqual(result1.PasswordSalt, result2.PasswordSalt);
        }

        [Fact]
        public void VerifyPasswordHash_ValidPassword_ReturnsTrue()
        {
            // Arrange
            string password = "TestPassword123";
            var result = _hashService.CreatePasswordHash(password);

            // Act
            var isValid = _hashService.VerifyPasswordHash(password, result.PasswordHash, result.PasswordSalt);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPasswordHash_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            string password = "TestPassword123";
            string wrongPassword = "WrongPassword";
            var result = _hashService.CreatePasswordHash(password);

            // Act
            var isValid = _hashService.VerifyPasswordHash(wrongPassword, result.PasswordHash, result.PasswordSalt);

            // Assert
            Assert.False(isValid);
        }
    }
}
