using Xunit;
using Moq;
using SchoolApp.UserAuthentication;

namespace TDDProjectDreamTeam.UserAuthentication
{
    public class HashingServiceTests
    {
        private readonly IHashingService _hashingService;

        public HashingServiceTests()
        {
            _hashingService = new HashingService();
        }

        [Fact]
        public void HashPassword_Should_Return_Hashed_String()
        {
            // Arrange
            var password = "SecurePassword123!";

            // Act
            var hashedPassword = _hashingService.HashPassword(password);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void HashPassword_Should_Produce_Different_Hashes_For_Same_Input()
        {
            // Arrange
            var password = "SecurePassword123!";

            // Act
            var hash1 = _hashingService.HashPassword(password);
            var hash2 = _hashingService.HashPassword(password);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_Should_Return_True_For_Valid_Password_And_Hash()
        {
            // Arrange
            var password = "SecurePassword123!";
            var hashedPassword = _hashingService.HashPassword(password);

            // Act
            var result = _hashingService.VerifyPassword(password, hashedPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_Should_Return_False_For_Invalid_Password_And_Hash()
        {
            // Arrange
            var password = "SecurePassword123!";
            var wrongPassword = "WrongPassword123!";
            var hashedPassword = _hashingService.HashPassword(password);

            // Act
            var result = _hashingService.VerifyPassword(wrongPassword, hashedPassword);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HashPassword_Should_Throw_Exception_For_Null_Input()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _hashingService.HashPassword(null!));
        }

        [Fact]
        public void VerifyPassword_Should_Throw_Exception_For_Null_Password()
        {
            // Arrange
            var hashedPassword = _hashingService.HashPassword("Password123!");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _hashingService.VerifyPassword(null!, hashedPassword));
        }
    }
}

